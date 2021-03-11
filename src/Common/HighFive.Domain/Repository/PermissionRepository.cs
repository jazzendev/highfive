using AutoMapper;
using Dapper;
using HighFive.Core.DomainModel;
using HighFive.Core.Model;
using HighFive.Core.Repository;
using HighFive.Domain.DomainModel;
using HighFive.Domain.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace HighFive.Domain.Repository
{
    public class PermissionRepository : CRUDRepository<Permission, PermissionDto>, IPermissionRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public PermissionRepository(IConnectionFactory connectionFactory,
            ILogger<PermissionRepository> logger,
            IMapper mapper,
            IPrincipal principal) : base(connectionFactory, logger, mapper, principal)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public IEnumerable<PermissionDto> GetPermissionCandidates(string roleId = "")
        {
            using (var connection = GetConnection())
            {
                var authLevel = CurrentAuthLevel;

                if (!string.IsNullOrEmpty(roleId))
                {
                    var targetRole = connection.QueryFirstOrDefault<Role>(@"SELECT TOP 1 * FROM [_Roles] WHERE @roleId=Id AND IsValid=1", new { roleId });
                    if (targetRole != null)
                    {
                        authLevel = (AppAuthLevel)Math.Min((int)CurrentAuthLevel, (int)targetRole.AuthLevel);
                    }
                }


                switch (authLevel)
                {
                    case AppAuthLevel.SuperAdmin:
                        authLevel = AppAuthLevel.SuperOperation;
                        break;
                    case AppAuthLevel.TenantAdmin:
                        authLevel = AppAuthLevel.TenantOperation;
                        break;
                }

                var data = connection.Query<PermissionDto>(@"
SELECT p.* FROM [_Permissions] p
WHERE p.IsValid=1 AND @authLevel<=p.AuthLevel", new { authLevel });

                return data;
            }
        }

        #region protected permission from modification
        public override IEnumerable<PermissionDto> GetAll(bool? isValid)
        {
            throw new UnauthorizedAccessException("Not allowed to get all permissions.");
        }

        public override PermissionDto Create(PermissionDto dto)
        {
            throw new UnauthorizedAccessException("Not allowed to create new permissions.");
        }

        public override PermissionDto Update(PermissionDto dto)
        {
            throw new UnauthorizedAccessException("Not allowed to modify permissions.");
        }

        public override bool Remove(string id)
        {
            throw new UnauthorizedAccessException("Not allowed to remove permissions.");
        }

        public override bool Delete(string id)
        {
            throw new UnauthorizedAccessException("Not allowed to delete permissions.");
        }
        #endregion
    }
}
