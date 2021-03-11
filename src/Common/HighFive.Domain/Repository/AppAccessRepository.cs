using AutoMapper;
using Dapper;
using HighFive.Core.DomainModel;
using HighFive.Core.Model;
using HighFive.Core.Repository;
using HighFive.Domain.DomainModel;
using HighFive.Domain.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace HighFive.Domain.Repository
{
    public class AppAccessRepository : DatabaseRepository, IAppAccessRepository
    {
        private readonly ILogger _logger;

        public AppAccessRepository(IConnectionFactory connectionFactory,
            ILogger<AppAccessRepository> logger) : base(connectionFactory, logger)
        {
            _logger = logger;
        }

        public AccountAccessDto GetAppAccess(string userId, string tenantId)
        {
            using (var connection = GetConnection())
            {
                var tenant = connection.QueryFirstOrDefault<TenantDto>(@"
SELECT TOP 1 * FROM [_Tenants] WHERE @tenantId=Id AND IsValid=1
", new { tenantId });


                if (tenant == null || !tenant.IsAllowed || tenant.IsBlocked)
                {
                    return null;
                }

                var account = connection.QueryFirstOrDefault<AccountAccessDto>(@"
SELECT TOP 1 a.*, td.Name DefaultTenantName,
    t.Id TenantId, t.Name TenantName, ta.AccessStartTime, ta.AccessEndTime, ta.BlockStartTime, ta.BlockEndTime
FROM [_Accounts] a
    INNER JOIN [_Tenants] td on td.Id=a.DefaultTenantId
    INNER JOIN [_TenantAccounts] ta on ta.AccountId=a.Id
    INNER JOIN [_Tenants] t on t.Id=ta.TenantId
WHERE @userId=a.Id AND @tenantId=t.Id AND a.IsValid=1 AND t.IsValid=1 AND ta.IsValid=1", new { userId, tenantId });

                if (account == null || !account.IsAllowed || account.IsBlocked)
                {
                    return null;
                }

                // get roles
                var roles = connection.Query<AccountAccessRoleDto>(@"
SELECT r.*, STUFF((
    SELECT ','+rp.PermissionId
    FROM [_RolePermissions] rp
    WHERE rp.RoleId=r.Id AND rp.IsValid=1
    FOR XML PATH('')
  ), 1, 1, '') as PermissionStr
FROM [_Accounts] a
    INNER JOIN [_AccountRoles] ar on ar.AccountId=a.Id
    INNER JOIN [_Roles] r on r.Id=ar.RoleId
WHERE @userId=a.Id AND @tenantId=r.TenantId AND a.IsValid=1 AND r.IsValid=1 AND ar.IsValid=1", new { userId, tenantId });
                account.Roles = roles;

                // get tenant services
                var data = connection.Query<TenantServiceDto>(@"
SELECT s.*
FROM [_Tenants] t
    INNER JOIN [_TenantServices] s on t.Id=s.TenantId
WHERE @tenantId=t.Id AND t.IsValid=1 AND s.IsValid=1", new { tenantId });

                var services = (AppServiceCode[])Enum.GetValues(typeof(AppServiceCode));

                var timestamp = DateTime.UtcNow;

                var q = from s in services
                        join d in data on s equals d.AppServiceCode into dg
                        from ts in dg.DefaultIfEmpty(new TenantServiceDto() { ServiceCode = s.ToString() })
                        where s != AppServiceCode.NA && ts.IsAllowed && !ts.IsBlocked
                        select ts.AppServiceCode;

                account.Services = q;
                return account;
            }
        }
    }
}
