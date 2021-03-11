using AutoMapper;
using Dapper;
using HighFive.Core.DomainModel;
using HighFive.Core.Model;
using HighFive.Core.Repository;
using HighFive.Core.Utility;
using HighFive.Domain.DomainModel;
using HighFive.Domain.Model;
using HighFive.Extensions.Dapper.Contrib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace HighFive.Domain.Repository
{
    public class RoleRepository : CRUDRepository<Role, RoleDto>, IRoleRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RoleRepository(IConnectionFactory connectionFactory,
            ILogger<RoleRepository> logger,
            IMapper mapper,
            IPrincipal principal) : base(connectionFactory, logger, mapper, principal)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public override IEnumerable<RoleDto> GetAll(bool? isValid = true)
        {
            using (var connection = GetConnection())
            {

                var command = @"
SELECT r.*
FROM [_Roles] r
WHERE @authLevel<=r.AuthLevel
    AND (@isValid IS NULL OR @isValid=IsValid)
    AND (r.TenantId is NULL OR @tenantId=r.TenantId)";

                var data = connection.Query<RoleDto>(command, new { tenantId = CurrentTenantId, authLevel = CurrentAuthLevel, isValid });

                return data;
            }
        }

        public override RoleDto Create(RoleDto dto)
        {
            using (var connection = GetConnection())
            {
                try
                {
                    Role entity = _mapper.Map<Role>(dto);
                    var authLevel = CurrentAuthLevel;
                    switch (authLevel)
                    {
                        case AppAuthLevel.SuperAdmin:
                            authLevel = AppAuthLevel.SuperOperation;
                            break;
                        case AppAuthLevel.TenantAdmin:
                            authLevel = AppAuthLevel.TenantOperation;
                            break;
                    }
                    entity.AuthLevel = authLevel;
                    entity.TenantId = CurrentTenantId;
                    entity.Type = RoleType.Custom;

                    connection.MonitorInsert(CurrentUserId, DateTime.UtcNow, entity);

                    var result = connection.QueryFirstOrDefault<RoleDto>(SINGLE_QUERY_COMMAND, new { Id = entity.Id });
                    return result;
                }
                catch (Exception e)
                {
                    _logger?.LogError(e.Message);
                    return null;
                }
            }
        }

        public IEnumerable<RoleDto> GetRoles(string tenantId)
        {
            using (var connection = GetConnection())
            {

                var command = @"
SELECT r.*
FROM [_Roles] r
WHERE r.TenantId is NULL OR @tenantId=r.TenantId";

                var data = connection.Query<RoleDto>(command, new { tenantId });

                return data;
            }
        }
    }
}
