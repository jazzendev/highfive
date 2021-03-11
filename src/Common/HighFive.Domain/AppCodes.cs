using HighFive.Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Domain
{
    public enum AppServiceCode
    {
        NA = 0,
        [Description(Name ="基础", Description = "基础信息管理")]
        Basic = 1,
        [Description(Name = "工作流", Description = "工作流管理")]
        Workflow = 2
    }

    /// <summary>
    /// Codes mapping to DB store
    /// </summary>
    public enum AppRoleCode
    {
        NA = 0,

        [Description(Name = "超级管理员", Code = "00001")]
        SuperAdmin = 1,
        [Description(Name = "超级运维", Code = "00101")]
        SuperOperator = 101,
        [Description(Name = "域管理员", Code = "10001")]
        TenantAdmin = 10001,
        [Description(Name = "域运维", Code = "10101")]
        TenantOperator = 10101
    }

    /// <summary>
    /// Codes mapping to DB store
    /// </summary>
    public enum AppPermissionCode
    {
        NA = 0,

        Central_Operation = 1,
        Central_Tenant_Managment = 2,
        Central_Account_Management = 3,
        Central_Role_Management = 4,

        Tenant_Operation = 10001,
        Tenant_Account_Management = 10002,
        Tenant_Role_Management = 10003
    }
}
