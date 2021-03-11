using AutoMapper;
using Dapper;
using HighFive.Core.Repository;
using HighFive.Core.Security;
using HighFive.Domain.DomainModel;
using HighFive.Domain.Model;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Security.Principal;
using Microsoft.Extensions.Logging;
using HighFive.Core.Provider;
using Microsoft.Extensions.Configuration;
using HighFive.Core.DomainModel;
using HighFive.Extensions.Dapper.Contrib;
using HighFive.Core.Utility;
using Microsoft.Extensions.Options;
using HighFive.Core.Configuration;
using System.Data.Common;

namespace HighFive.Domain.Repository
{
    public class AccountRepository : CRUDRepository<Account, AccountDto>, IAccountRepository
    {
        private readonly DefaultValueConfig _config;
        private readonly IJwtProvider _jwtProvider;
        private readonly ILogger _logger;
        private readonly SimplePasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public AccountRepository(IConnectionFactory connectionFactory,
            IOptions<DefaultValueConfig> config,
            IJwtProvider jwtProvider,
            SimplePasswordHasher passwordHasher,
            ILogger<AccountRepository> logger,
            IMapper mapper,
            IPrincipal principal) : base(connectionFactory, logger, mapper, principal)
        {
            _config = config.Value;
            _jwtProvider = jwtProvider;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _mapper = mapper;
        }

        public PaginationResult<AccountDisplayDto> Query(AccountQuery query)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var columnsCommand = @"
SELECT a.*, t.Name TenantName, ta.Id TenantAccountId,
    ta.AccessStartTime, ta.AccessEndTime, ta.BlockStartTime, ta.BlockEndTime,
    STUFF((
    SELECT ','+r.Name
    FROM [_Roles] r INNER JOIN [_AccountRoles] ar ON r.Id=ar.RoleId
    WHERE r.TenantId=t.Id AND ar.AccountId=a.Id AND r.IsValid=1 AND ar.IsValid=1
    FOR XML PATH('')
  ), 1, 1, '') as RoleStr";
                var queryCommand = @"
FROM [_Accounts] a
    INNER JOIN [_TenantAccounts] ta on ta.AccountId=a.Id
    INNER JOIN [_Tenants] t on t.Id=ta.TenantId
WHERE a.IsValid=1 AND t.IsValid=1 AND ta.IsValid=1
    AND (@Username is NULL or a.Username like @Username)
    AND (@TenantId is NULL or @TenantId=t.Id)
    AND (@TenantName is NULL or t.Name like @TenantName)";

                var pagingCommand = query.Page == -1 ? "" : @"
ORDER BY a.Id DESC
OFFSET @Offset ROWS
FETCH NEXT @Size ROWS ONLY; ";

                query.Username = string.IsNullOrWhiteSpace(query.Username) ? null : $"%{query.Username.Trim()}%";
                query.TenantName = string.IsNullOrWhiteSpace(query.TenantName) ? null : $"%{query.TenantName.Trim()}%";

                var data = connection.Query<AccountDisplayDto>($@"{columnsCommand} {queryCommand} {pagingCommand}", query).ToList();
                var count = connection.ExecuteScalar<long>($@"SELECT COUNT(a.Id) {queryCommand}", query);

                var result = CombinePaginationResult(query, data, count);

                return result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public AccountDto Verify(string username, string passwordHash)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var command = @"
SELECT TOP 1 a.*
FROM [_Accounts] a
    INNER JOIN [_TenantAccounts] ta on a.Id=ta.AccountId
    INNER JOIN [_Tenants] t on t.Id=ta.TenantId
WHERE a.IsValid=1 AND t.IsValid=1 AND ta.IsValid=1 AND @username=a.Username";

                var account = connection.QueryFirstOrDefault<AccountDto>(command, new { username });

                if (account == null)
                {
                    return null;
                }

                var verifyResult = _passwordHasher.VerifyHashedPassword(account.PwdHash, passwordHash);

                if (!verifyResult)
                {
                    return null;
                }

                return account;
            }
        }

        /// <summary>
        /// Get Account with default tenant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override AccountDto Get(string id)
        {
            return Get(id, null);
        }

        /// <summary>
        /// Get Account with specific tenant
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public AccountDto Get(string id, string tenantId)
        {
            using (var connection = GetConnection())
            {
                var tenantCondition = string.IsNullOrEmpty(tenantId) ? "a.DefaultTenantId=t.Id" : "@tenantId=t.Id";
                var command = @$"
SELECT a.*, (SELECT TOP 1 Name FROM [_Tenants] WHERE Id=a.DefaultTenantId) [DefaultTenantName],
    t.Id TenantId, t.Name TenantName, ta.AccessStartTime, ta.AccessEndTime, ta.BlockStartTime, ta.BlockEndTime
FROM [_Accounts] a
    INNER JOIN [_TenantAccounts] ta on ta.AccountId=a.Id
    INNER JOIN [_Tenants] t on t.Id=ta.TenantId
WHERE @id=a.Id AND {tenantCondition} 
    AND a.IsValid=1 AND t.IsValid=1 AND ta.IsValid=1";

                var dto = connection.QueryFirstOrDefault<AccountDto>(command, new { id, tenantId });
                dto.Roles = GetRoles(id, tenantId, connection);

                return dto;
            }
        }

        public override AccountDto Create(AccountDto dto)
        {
            var timestamp = DateTime.UtcNow;

            using (var connection = GetConnection())
            {
                connection.Open();

                using (var transcation = connection.BeginTransaction())
                {
                    try
                    {
                        // create account
                        var account = _mapper.Map<Account>(dto);
                        connection.MonitorInsert(CurrentUserId, timestamp, account, transcation);

                        // create account-tenant relationship
                        var tenantAccount = _mapper.Map<Account>(dto);
                        connection.MonitorInsert(CurrentUserId, timestamp, tenantAccount, transcation);

                        // create account-roles relationship
                        foreach (var r in dto.Roles)
                        {
                            var accountRole = new AccountRole()
                            {
                                AccountId = account.Id,
                                RoleId = r.Id
                            };

                            connection.MonitorInsert(CurrentUserId, timestamp, accountRole, transcation);
                        }

                        transcation.Commit();
                        return dto;
                    }
                    catch (Exception e)
                    {
                        _logger?.LogError(e.Message);
                        transcation.Rollback();
                        return null;
                    }
                }
            }
        }

        public override AccountDto Update(AccountDto dto)
        {
            var timestamp = DateTime.UtcNow;

            using (var connection = GetConnection())
            {
                connection.Open();

                using (var transcation = connection.BeginTransaction())
                {
                    try
                    {
                        // set some columns thant wont change during updating
                        var previous = connection.QueryFirstOrDefault<Account>("SELECT TOP 1 * FROM [_Accounts] WHERE @id=Id", new { id = dto.Id }, transcation);
                        dto.PwdHash = previous.PwdHash;

                        var account = _mapper.Map<Account>(dto);
                        connection.MonitorUpdate(CurrentUserId, timestamp, account, transcation);
                        var tenantAccount = _mapper.Map<TenantAccount>(dto);
                        connection.MonitorUpdate(CurrentUserId, timestamp, tenantAccount, transcation);

                        var roles = SetRoles(dto.Id, dto.TenantId, dto.Roles.Select(r => r.RoleId), connection, transcation);
                        dto.Roles = roles;

                        transcation.Commit();

                        return dto;
                    }
                    catch (Exception e)
                    {
                        _logger?.LogError(e.Message);
                        transcation.Rollback();
                        return null;
                    }
                }
            }
        }

        //AccountDto UpdateInfo(AccountDto dto);
        public bool UpdatePassword(string id, string oldPassword, string newPassword)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var command = @"
SELECT a.*
FROM [_Accounts] a
WHERE @id=a.Id AND a.IsValid=1";

                var login = connection.QueryFirstOrDefault<Account>(command, new { Id = id });

                if (login == null)
                {
                    return false;
                }

                var verifyResult = _passwordHasher.VerifyHashedPassword(login.PwdHash, oldPassword);

                if (!verifyResult)
                {
                    return false;
                }

                var newPasswordHash = _passwordHasher.HashPassword(newPassword);

                login.PwdHash = newPasswordHash;
                login.IsPwdNeedChange = false;

                return connection.MonitorUpdate(CurrentUserId, DateTime.UtcNow, login);
            }
        }


        public bool ResetPassword(string id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var command = @"
SELECT a.*
FROM [_Accounts] a
WHERE @id=a.Id AND a.IsValid=1";

                var login = connection.QueryFirstOrDefault<Account>(command, new { Id = id });

                var newPasswordHash = _passwordHasher.HashPassword(_config.Password);

                login.PwdHash = newPasswordHash;
                login.IsPwdNeedChange = true;

                return connection.MonitorUpdate(CurrentUserId, DateTime.UtcNow, login);
            }
        }

        //TenantAccountDto SetAccountAccess(TenantAccountDto dto);

        public bool CheckUsernameAvaiable(string accountId, string username)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var command = @"
SELECT TOP 1 a.*
FROM [_Accounts] a
WHERE @username=Username AND a.IsValid=1 AND (@accountId is null OR l.Id!=@accountId)";

                var account = connection.QueryFirstOrDefault<Account>(command, new { username, accountId });

                return account == null;
            }
        }

        public bool CheckNameAvaiable(string accountId, string name)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var command = @"
SELECT TOP 1 a.*
FROM [_Accounts] a
WHERE @name=name AND a.IsValid=1 AND (@accountId is null OR l.Id!=@accountId)";

                var account = connection.QueryFirstOrDefault<Account>(command, new { name, accountId });

                return account == null;
            }
        }

        public IEnumerable<AccountRoleDto> GetRoles(string accountId, string tenantId)
        {
            using (var connection = GetConnection())
            {
                return GetRoles(accountId, tenantId, connection);
            }
        }

        IEnumerable<AccountRoleDto> GetRoles(string accountId, string tenantId, DbConnection connection, DbTransaction transaction = null)
        {
            var command = @"
SELECT ar.*, r.Name RoleName, r.TenantId
FROM [_Roles] r
    INNER JOIN [_AccountRoles] ar on r.Id=ar.RoleId
WHERE @AccountId=ar.AccountId AND ar.IsValid=1 AND r.IsValid=1 
    AND (r.TenantId is NULL OR @tenantId=r.TenantId)
    AND @AuthLevel<=r.AuthLevel";

            var dtos = connection.Query<AccountRoleDto>(command, new { AccountId = accountId, tenantId = tenantId, authLevel = CurrentAuthLevel }, transaction);

            return dtos;
        }


        IEnumerable<AccountRoleDto> SetRoles(string accountId, string tenantId, IEnumerable<string> roleIds, DbConnection connection, DbTransaction transaction)
        {
            // setup an unique timestamp for inserting and updating
            var timestamp = DateTime.UtcNow;

            // check roles first, prevent higher AuthLevel roles to be set
            var filtered = connection.Query<string>(@"
SELECT r.*
FROM [_Roles] r
WHERE r.Id IN @Ids AND r.IsValid=1 AND @AuthLevel<=r.AuthLevel 
    AND (r.TenantId is NULL OR @tenantId=r.TenantId)", new { Ids = roleIds, tenantId = tenantId, authLevel = CurrentAuthLevel }, transaction);

            // get all current selected roles
            var currentSelected = connection.Query<string>(@"
SELECT r.Id
FROM [_Roles] r
    INNER JOIN [_AccountRoles] ar on r.Id=ar.RoleId
WHERE @AccountId=ar.AccountId AND ar.IsValid=1 AND r.IsValid=1 
    AND (r.TenantId is NULL OR @tenantId=r.TenantId)
    AND @AuthLevel<=r.AuthLevel", new { AccountId = accountId, tenantId = tenantId, authLevel = CurrentAuthLevel }, transaction);

            // filter out roles need deleting
            var ids2Del = currentSelected.Except(filtered);
            connection.Execute(@"
UPDATE ar
SET ar.IsValid=0, ar.EditorId=@EditorId, ar.LastEditTime=@Timestamp
FROM [_AccountRoles] ar
WHERE @AccountId=ar.AccountId AND ar.RoleId in @Ids",
new { AccountId = accountId, Ids = ids2Del, Timestamp = timestamp, EditorId = CurrentUserId },
transaction);

            // add new selected permissions
            var items2add = filtered.Except(currentSelected).Select(id =>
            {
                return new AccountRole()
                {
                    Id = IdHelper.NewId(),
                    AccountId = accountId,
                    RoleId = id,
                    CreatorId = CurrentUserId,
                    CreationTime = timestamp,
                    IsValid = true
                };
            }).ToArray();

            connection.Insert(items2add, transaction);

            var data = GetRoles(accountId, tenantId, connection, transaction);
            return data;
        }


        public bool Disable(string id)
        {
            throw new NotImplementedException();
        }

        public bool Enable(string id)
        {
            throw new NotImplementedException();
        }
    }
}
