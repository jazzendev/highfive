using HighFive.Core.DomainModel;
using HighFive.Core.Repository;
using HighFive.Domain.DomainModel;
using System.Collections.Generic;

namespace HighFive.Domain.Repository
{
    public interface ITenantServiceRepository : ICRUDRepository<TenantServiceDto>
    {
        IEnumerable<TenantServiceDto> GetServices4Tenant(string tenantId);
    }
}
