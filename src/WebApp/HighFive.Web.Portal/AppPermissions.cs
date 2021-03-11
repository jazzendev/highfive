using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HighFive.Web.Portal
{
    public static class AppPermissions
    {
        public const string Central_Operation = "00001";
        public const string Central_Tenant_Managment = "00002";
        public const string Central_Account_Management = "00003";
        public const string Central_Role_Management = "00004";
        public const string Tenant_Operation = "10001";
        public const string Tenant_Account_Management = "10002";
        public const string Tenant_Role_Management = "10003";
    }
}
