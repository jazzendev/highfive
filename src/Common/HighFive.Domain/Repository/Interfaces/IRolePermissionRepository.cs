using HighFive.Core.DomainModel;
using HighFive.Core.Repository;
using HighFive.Domain.DomainModel;
using System.Collections.Generic;

namespace HighFive.Domain.Repository
{
    public interface IRolePermissionRepository
    {
        IEnumerable<RolePermissionDto> GetRolePermissions(string roleId);
        IEnumerable<RolePermissionDto> SetRolePermissions(string roleId, IEnumerable<string> permissionIds);
    }
}
