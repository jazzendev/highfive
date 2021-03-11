using HighFive.Core.DomainModel;
using System;

namespace HighFive.Domain.DomainModel
{
    public class TenantQuery : PaginationQuery
    {
        public string RootId { get; set; }
        public string Name { get; set; }

        public bool? IsValid { get; set; }
    }

    public class TenantSimpleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class TenantDto : AppAccessDto
    {
        public string RootId { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
    }

    public class TenantAccountDto : AppAccessDto
    {
        public string TenantId { get; set; }
        public string AccountId { get; set; }
    }

    public class TenantServiceDto : AppAccessDto
    {
        public string TenantId { get; set; }
        public string ServiceCode { get; set; }
        public AppServiceCode AppServiceCode
        {
            get
            {
                return (AppServiceCode)Enum.Parse(typeof(AppServiceCode), ServiceCode);
            }
        }
    }
}
