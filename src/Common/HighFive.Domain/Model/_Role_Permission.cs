using HighFive.Extensions.Dapper.Contrib;
using HighFive.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


namespace HighFive.Domain.Model
{
    public enum RoleType
    {
        NA = 0,
        /// <summary>
        /// describe pre-defined roles, readonly
        /// </summary>
        System = 1,
        /// <summary>
        /// describe custom created roles
        /// </summary>
        Custom = 2
    }

    public enum PermissionType
    {
        NA = 0,
        /// <summary>
        /// describe pre-defined roles, readonly
        /// </summary>
        System = 1,
        /// <summary>
        /// describe custom created roles
        /// not going to open this otion currently
        /// </summary>
        //Custom = 2
    }

    [Table("_Roles")]
    public class Role : AppRole
    {
        [DbReadonly]
        public string TenantId { get; set; }
        public string Description { get; set; }
        [DbReadonly]
        public RoleType Type { get; set; }
    }


    [Table("_Permissions")]
    public class Permission : AppPermission
    {
        public string Description { get; set; }
        [DbReadonly]
        public PermissionType Type { get; set; }
    }

    [Table("_RolePermissions")]
    public class RolePermission : MonitorModel<string>
    {
        public string PermissionId { get; set; }
        public string RoleId { get; set; }
    }
}
