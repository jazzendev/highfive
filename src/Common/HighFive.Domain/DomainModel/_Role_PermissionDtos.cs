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
    public class RoleDto : MonitorDto
    {
        public string TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AppAuthLevel AuthLevel { get; set; }
        public RoleType Type { get; set; }
    }

    public class PermissionDto : MonitorDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AppAuthLevel AuthLevel { get; set; }
        public PermissionType Type { get; set; }
    }

    public class RolePermissionDto : MonitorDto
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public AppAuthLevel RoleAuthLevel { get; set; }

        public string PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionDescription { get; set; }
        public AppAuthLevel PermissionAuthLevel { get; set; }
    }
}
