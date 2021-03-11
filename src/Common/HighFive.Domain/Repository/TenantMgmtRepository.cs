using AutoMapper;
using Dapper;
using HighFive.Extensions.Dapper.Contrib;
using HighFive.Core.DomainModel;
using HighFive.Core.Repository;
using HighFive.Domain.DomainModel;
using HighFive.Domain.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using HighFive.Core.Utility;
using Microsoft.Extensions.Options;
using HighFive.Core.Configuration;
using HighFive.Core.Security;

namespace HighFive.Domain.Repository
{
    public class TenantMgmtRepository : CRUDRepository<Tenant, TenantDto>, ITenantMgmtRepository
    {
        private readonly SimplePasswordHasher _passwordHasher;
        private readonly DefaultValueConfig _config;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TenantMgmtRepository(IConnectionFactory connectionFactory,
            SimplePasswordHasher passwordHasher,
            IOptions<DefaultValueConfig> config,
            ILogger<TenantMgmtRepository> logger,
            IMapper mapper,
            IPrincipal principal) : base(connectionFactory, logger, mapper, principal)
        {
            _passwordHasher = passwordHasher;
            _logger = logger;
            _mapper = mapper;
            _config = config.Value;
        }

        public PaginationResult<TenantDto> Query(TenantQuery query)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var columnsCommand = @"
SELECT t.*";
                var queryCommand = @"
FROM [_Tenants] t
WHERE (@IsValid is NULL or @IsValid=t.IsValid) 
    AND (@Name is NULL OR t.Name like @Name)";

                var pagingCommand = query.Page == -1 ? "" : @"
ORDER BY t.Id DESC
OFFSET @Offset ROWS
FETCH NEXT @Size ROWS ONLY; ";

                query.Name = string.IsNullOrWhiteSpace(query.Name) ?
                    null : $"%{query.Name.Trim()}%";
                var data = connection.Query<TenantDto>($@"{columnsCommand} {queryCommand} {pagingCommand}", query);
                var count = connection.ExecuteScalar<long>($@"SELECT COUNT(t.Id) {queryCommand}", query);

                var result = CombinePaginationResult(query, data, count);

                return result;
            }
        }

        public override TenantDto Create(TenantDto dto)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var timestamp = DateTime.UtcNow;

                        var tenant = _mapper.Map<Tenant>(dto);
                        connection.MonitorInsert(CurrentUserId, timestamp, tenant, transaction);

                        if (_config.InitTenantAdmin)
                        {
                            var admin = new Account()
                            {
                                DefaultTenantId = tenant.Id,
                                Name = _config.AdminName,
                                Username = $"{_config.AdminUsername}@{tenant.Domain}",
                                PwdHash = _passwordHasher.HashPassword(_config.AdminPassword),
                                IsPwdNeedChange = true,
                                AvatarUrl = null,
                                Email = null,
                            };
                            connection.MonitorInsert(CurrentUserId, timestamp, admin, transaction);

                            var tenantAccout = new TenantAccount()
                            {
                                TenantId = tenant.Id,
                                AccountId = admin.Id,
                                //AccessStartTime = null,
                                //AccessEndTime = null
                            };
                            connection.MonitorInsert(CurrentUserId, timestamp, tenantAccout, transaction);

                            // assign tenant admin role
                            var tenantAdminRole = new AccountRole()
                            {
                                AccountId = admin.Id,
                                RoleId = AppRoleCode.TenantAdmin.Code()
                            };
                            connection.MonitorInsert(CurrentUserId, timestamp, tenantAdminRole, transaction);

                            // assign tenant operator role
                            var tenantOperatorRole = new AccountRole()
                            {
                                AccountId = admin.Id,
                                RoleId = AppRoleCode.TenantOperator.Code()
                            };
                            connection.MonitorInsert(CurrentUserId, timestamp, tenantOperatorRole, transaction);
                        }

                        TenantService ts = new TenantService()
                        {
                            TenantId = tenant.Id,
                            ServiceCode = ((int)AppServiceCode.Basic).ToString(),
                            AccessStartTime = DateTime.UtcNow.Date,
                            AccessEndTime = DateTime.UtcNow.Date.AddYears(1)
                        };
                        connection.MonitorInsert(CurrentUserId, timestamp, ts, transaction);

                        var result = connection.QueryFirstOrDefault<TenantDto>(SINGLE_QUERY_COMMAND, new { Id = tenant.Id }, transaction);
                        transaction.Commit();

                        return result;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        _logger?.LogError(e.Message);
                        return null;
                    }
                }
            }
        }

        public IEnumerable<AccountDto> GetAccounts(string tenantId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                var command = @"
SELECT * FROM [_TenantAccounts] WHERE @tenantId=TenantId AND IsValid=1
";
                var data = connection.Query<AccountDto>(command, new { tenantId });
                return data;
            }
        }
    }
}
