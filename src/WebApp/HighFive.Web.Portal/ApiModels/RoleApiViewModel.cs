using HighFive.Core.Model;
using HighFive.Domain.DomainModel;
using HighFive.Domain.Model;
using HighFive.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HighFive.Web.Portal.ApiModels
{
    public class RoleApiViewModel : MonitorViewModel
    {
        public string TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AppAuthLevel AuthLevel { get; set; }
        [JsonIgnore]
        public RoleType Type { get; set; }
    }

    public class RoleUpsertApiViewModel : RoleApiViewModel
    {
        public IEnumerable<string> Permissions { get; set; }
    }

    public class PermissionSelectionApiViewModel
    {
        public IEnumerable<PermissionApiViewModel> PermissionCandidates { get; set; }
        public IEnumerable<SelectedPermissionApiViewModel> PermissionSelected { get; set; }
    }

    public class SelectedPermissionApiViewModel
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string PermissionId { get; set; }
    }

    public class RolePermissionApiViewModel : MonitorViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public AppAuthLevel RoleAuthLevel { get; set; }

        public string PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionDescription { get; set; }
        public AppAuthLevel PermissionAuthLevel { get; set; }
    }

    public class PermissionApiViewModel : MonitorViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AppAuthLevel AuthLevel { get; set; }
        [JsonIgnore]
        public PermissionType Type { get; set; }
    }
}
