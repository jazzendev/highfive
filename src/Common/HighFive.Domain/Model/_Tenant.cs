using HighFive.Extensions.Dapper.Contrib;
using HighFive.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HighFive.Domain.Model
{
    [Table("_Tenants")]
    public class Tenant : AppAccessModel<string>
    {
        public string RootId { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
    }

    [Table("_TenantAccounts")]
    public class TenantAccount : AppAccessModel<string>
    {
        [DbReadonly]
        public string TenantId { get; set; }
        [DbReadonly]
        public string AccountId { get; set; }
    }

    [Table("_TenantServices")]
    public class TenantService : AppAccessModel<string>
    {
        [DbReadonly]
        public string TenantId { get; set; }
        [DbReadonly]
        public string ServiceCode { get; set; }
    }
}
