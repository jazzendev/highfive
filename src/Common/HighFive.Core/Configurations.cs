using HighFive.Core.Provider;
using System;
using System.Collections.Generic;
using System.Text;

namespace HighFive.Core.Configuration
{
    public class TokenConfig
    {
        public string SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string RsaPrivateKey { get; set; }
        public string RsaPublicKey { get; set; }
        public EncryptMethod EncryptMethod { get; set; }
    }

    public class DefaultValueConfig
    {
        public bool InitTenantAdmin { get; set; }
        public string AdminName { get; set; }
        public string AdminUsername { get; set; }
        public string AdminPassword { get; set; }
        public string Password { get; set; }
        public int PageSize { get; set; }
    }

    public class ApiDefaultValueConfig
    {
        public string ApiRouteNewId { get; set; }
    }

    public class DbConnectionStringConfig
    {
        public string DefaultConnection { get; set; }
        public string ProductionConnection { get; set; }
    }

    public class AzureStorageConnectionStringConfig
    {
        public string DefaultConnection { get; set; }
        public string ProductionConnection { get; set; }
    }
}