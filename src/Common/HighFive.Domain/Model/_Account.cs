using HighFive.Extensions.Dapper.Contrib;
using HighFive.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HighFive.Domain.Model
{
    public enum Roles
    {
        [Description("超级管理员")]
        SuperAdmin = 1,
        [Description("域管理员")]
        TenanatAdmin = 10,
        [Description("运维")]
        Operator = 100,
        [Description("普通用户")]
        User = 1000
    }

    [Table("_Accounts")]
    public class Account : AppUser
    {
        public string DefaultTenantId { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool IsPwdNeedChange { get; set; }

        public string AvatarUrl { get; set; }
    }


    [Table("_AccountRoles")]
    public class AccountRole : MonitorModel<string>
    {
        public string AccountId { get; set; }
        public string RoleId { get; set; }
    }
}
