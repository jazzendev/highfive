using HighFive.Core.DomainModel;
using HighFive.Core.Model;
using HighFive.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Domain.DomainModel
{
    public class AccountAccessDto : AppAccessDto
    {
        #region account
        public string Name { get; set; }
        public bool IsPwdNeedChange { get; set; }
        public string AvatarUrl { get; set; }
        #endregion

        #region default tenant
        public string DefaultTenantId { get; set; }
        public string DefaultTenantName { get; set; }
        #endregion

        #region current tenant
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public string TenantAccountId { get; set; }
        #endregion

        #region current tenant roles
        public IEnumerable<AccountAccessRoleDto> Roles { get; set; }
        #endregion

        #region current tenant services
        public IEnumerable<AppServiceCode> Services { get; set; }
        #endregion
    }

    public class AccountAccessRoleDto {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TenantId { get; set; }
        public string Description { get; set; }
        public AppAuthLevel AuthLevel { get; set; }

        public string PermissionStr { get; set; }
        public IEnumerable<AppPermissionCode> Permissions
        {
            get
            {
                if (string.IsNullOrEmpty(this.PermissionStr))
                {
                    return new AppPermissionCode[] { };
                }
                var roles = this.PermissionStr
                    .Split(',')
                    .Select(r => (AppPermissionCode)int.Parse(r));
                return roles;
            }
        }
    }
}