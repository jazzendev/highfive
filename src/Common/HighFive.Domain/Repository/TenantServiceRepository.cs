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
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace HighFive.Domain.Repository
{
    public class TenantServiceRepository : CRUDRepository<TenantService, TenantServiceDto>, ITenantServiceRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TenantServiceRepository(IConnectionFactory connectionFactory,
            ILogger<TenantServiceRepository> logger,
            IMapper mapper,
            IPrincipal principal) : base(connectionFactory, logger, mapper, principal)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public IEnumerable<TenantServiceDto> GetServices4Tenant(string tenantId)
        {
            using (var connection = GetConnection())
            {
                var command = @"
SELECT s.*
FROM [_Tenants] t
    INNER JOIN [_TenantServices] s on t.Id=s.TenantId
WHERE @tenantId=t.Id AND t.IsValid=1 AND s.IsValid=1
";

                var data = connection.Query<TenantServiceDto>(command, new { tenantId });

                var services = (AppServiceCode[])Enum.GetValues(typeof(AppServiceCode));

                var q = from s in services
                        join d in data on s equals d.AppServiceCode into dg
                        from ts in dg.DefaultIfEmpty(new TenantServiceDto() { ServiceCode = s.ToString() })
                        where s != AppServiceCode.NA
                        select ts;

                return q;
            }
        }

        //public override TenantServiceDto Create(TenantServiceDto dto)
        //{
        //    return base.Create(dto);
        //}

        //public override TenantServiceDto Update(TenantServiceDto dto)
        //{
        //    return base.Update(dto);
        //}
    }
}
