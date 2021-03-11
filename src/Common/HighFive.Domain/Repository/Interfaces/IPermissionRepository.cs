using HighFive.Core.DomainModel;
using HighFive.Core.Repository;
using HighFive.Domain.DomainModel;
using System.Collections.Generic;

namespace HighFive.Domain.Repository
{
    public interface IPermissionRepository : ICRUDRepository<PermissionDto>
    {
        IEnumerable<PermissionDto> GetPermissionCandidates(string roleId = "");
    }
}
