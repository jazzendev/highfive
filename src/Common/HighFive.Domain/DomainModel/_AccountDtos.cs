using HighFive.Core.DomainModel;
using HighFive.Core.Model;
using HighFive.Core.Repository;
using HighFive.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Domain.DomainModel
{
    public class AccountQuery : PaginationQuery
    {
        public string TenantId { get; set; }
        public string TenantName { get; set; }

        public string Username { get; set; }
    }

    public class AccountSimpleDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
    }

    public class AccountDisplayDto : AppAccessDto
    {
        #region properties for current tenant
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public string TenantAccountId { get; set; }
        #endregion

        public string Username { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string PwdHash { get; set; }
        public bool IsPwdNeedChange { get; set; }

        public string AvatarUrl { get; set; }

        public string RoleStr { get; set; }
    }

    public class AccountDto : AccountDisplayDto
    {
        public string DefaultTenantId { get; set; }
        public string DefaultTenantName { get; set; }

        public IEnumerable<AccountRoleDto> Roles { get; set; }
    }    

    public class AccountRoleDto : MonitorDto
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string AccountId { get; set; }
        public string TenantId { get; set; }
    }
}
