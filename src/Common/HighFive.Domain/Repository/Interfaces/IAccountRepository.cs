using HighFive.Core.DomainModel;
using HighFive.Core.Repository;
using HighFive.Domain.DomainModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Domain.Repository
{
    public interface IAccountRepository : ICRUDRepository<AccountDto>
    {
        AccountDto Get(string id, string tenantId);
        PaginationResult<AccountDisplayDto> Query(AccountQuery query);
        AccountDto Verify(string username, string passwordHash);

        bool UpdatePassword(string id, string oldPassword, string newPassword);
        //TenantAccountDto SetAccountAccess(TenantAccountDto dto);

        bool ResetPassword(string id);
        bool CheckUsernameAvaiable(string accountId, string username);
        bool CheckNameAvaiable(string accountId, string name);

        //AccountDto SetTenantAccess(string accountId, string tenantId, DateTime? start, DateTime? end);

        IEnumerable<AccountRoleDto> GetRoles(string accountId, string tenantId);
        //IEnumerable<AccountRoleDto> SetRoles(string accountId, string tenantId, IEnumerable<string> roleIds);

        bool Enable(string id);
        bool Disable(string id);
    }
}
