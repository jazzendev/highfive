using HighFive.Core.DomainModel;
using HighFive.Core.Repository;
using HighFive.Domain.DomainModel;
using System.Collections.Generic;

namespace HighFive.Domain.Repository
{
    public interface ITenantMgmtRepository : ICRUDRepository<TenantDto>
    {
        PaginationResult<TenantDto> Query(TenantQuery query);
        IEnumerable<AccountDto> GetAccounts(string tenantId);
    }
}
