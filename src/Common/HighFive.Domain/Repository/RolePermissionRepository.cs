using AutoMapper;
using Dapper;
using HighFive.Extensions.Dapper.Contrib;
using HighFive.Core.DomainModel;
using HighFive.Core.Repository;
using HighFive.Core.Utility;
using HighFive.Domain.DomainModel;
using HighFive.Domain.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace HighFive.Domain.Repository
{
    public class RolePermissionRepository : MonitorRepository, IRolePermissionRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RolePermissionRepository(IConnectionFactory connectionFactory,
            ILogger<RolePermissionRepository> logger,
            IMapper mapper,
            IPrincipal principal) : base(connectionFactory, logger, principal)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public IEnumerable<RolePermissionDto> GetRolePermissions(string roleId)
        {
            using (var connection = GetConnection())
            {
                var command = @"
SELECT r.Id RoleId, r.Name RoleName, r.AuthLevel RoleAuthLevel,
    p.Id PermissionId, p.Name PermissionName, p.Description PermissionDescription, p.AuthLevel PermissionAuthLevel,
    rp.Id, rp.IsValid, rp.CreatorId, rp.CreationTime, rp.EditorId, rp.LastEditTime
FROM [_Roles] r 
    INNER JOIN [_RolePermissions] rp on rp.RoleId=r.Id
    INNER JOIN [_Permissions] p on p.Id=rp.PermissionId
WHERE @roleId=r.Id AND rp.IsValid=1 AND p.IsValid=1 AND r.AuthLevel<=p.AuthLevel";

                var data = connection.Query<RolePermissionDto>(command, new { roleId });

                return data;
            }
        }

        /// <summary>
        /// NOTICE! SHOULD NOT edit predifined roles & permissions.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionIds"></param>
        /// <returns></returns>
        public IEnumerable<RolePermissionDto> SetRolePermissions(string roleId, IEnumerable<string> permissionIds)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var targetRole = connection.QueryFirstOrDefault<Role>(@"SELECT TOP 1 * FROM [_Roles] WHERE @roleId=Id AND IsValid=1", new { roleId }, transaction);

                        if (targetRole == null
                            // prevent lower level user from modifying higher level data
                            || targetRole.AuthLevel < CurrentAuthLevel)
                        {
                            transaction.Commit();
                            return null;
                        }

                        // setup an unique timestamp for inserting and updating
                        var timestamp = DateTime.UtcNow;

                        // check permissions first, prevent higher AuthLevel permissions to be set
                        var filtered = connection.Query<string>(@"
SELECT p.Id
FROM [_Permissions] p
WHERE p.Id IN @Ids AND p.IsValid=1 AND @AuthLevel<=p.AuthLevel", new { Ids = permissionIds, AuthLevel = targetRole.AuthLevel }, transaction);

                        // get all current selected permissions
                        var currentSelected = connection.Query<string>(@"
SELECT p.Id
FROM [_RolePermissions] rp
    INNER JOIN [_Permissions] p on p.Id=rp.PermissionId
WHERE @Id=rp.RoleId AND rp.IsValid=1 AND p.IsValid=1 AND @AuthLevel<=p.AuthLevel", targetRole, transaction);

                        // filter out permissions need deleting
                        var ids2Del = currentSelected.Except(filtered);
                        connection.Execute(@"
UPDATE rp
SET rp.IsValid=0, rp.EditorId=@EditorId, rp.LastEditTime=@Timestamp
FROM [_RolePermissions] rp
WHERE @RoleId=rp.RoleId AND rp.PermissionId in @Ids",
new { RoleId = targetRole.Id, Ids = ids2Del, Timestamp = timestamp, EditorId = CurrentUserId },
transaction);

                        // add new selected permissions
                        var items2add = filtered.Except(currentSelected).Select(id =>
                        {
                            return new RolePermission()
                            {
                                Id = IdHelper.NewId(),
                                RoleId = targetRole.Id,
                                PermissionId = id,
                                CreatorId = CurrentUserId,
                                CreationTime = timestamp,
                                IsValid = true,
                            };
                        }).ToArray();

                        connection.Insert(items2add, transaction);   
                        var data = _mapper.Map<IEnumerable<RolePermissionDto>>(items2add);

                        transaction.Commit();
                        return data;
                    }
                    catch (Exception e)
                    {

                        transaction.Rollback();
                        _logger.LogError(e.StackTrace);
                        return null;
                    }
                }
            }
        }
    }
}
