using AutoMapper;
using AutoMapper.Configuration;
using HighFive.Core.DomainModel;
using HighFive.Core.Model;
using HighFive.Domain;
using HighFive.Domain.DomainModel;
using HighFive.Domain.Repository;
using HighFive.Web.Core;
using HighFive.Web.Core.Models;
using HighFive.Web.Core.Policies;
using HighFive.Web.Portal.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;

namespace HighFive.Web.Portal.ApiControllers
{
    [AppPermission(AppPermissionCode.Central_Tenant_Managment)]
    public class TenantController : AuthApiControllerBase
    {
        private readonly ILogger _logger;
        private readonly ITenantMgmtRepository _tr;
        private readonly ITenantServiceRepository _tsr;

        public TenantController(
            ITenantMgmtRepository tenantRepository,
            ITenantServiceRepository tenantServiceRepository,
            ILogger<TenantController> logger,
            IMapper mapper) : base(logger, mapper)
        {
            _tr = tenantRepository;
            _tsr = tenantServiceRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResultModel<PaginationResult<TenantApiViewModel>>), 200)]
        public IActionResult Query([FromQuery] TenantQuery query)
        {
            var tenant = TenantId;
            var results = _tr.Query(query);
            var vmResults = MapPaginationModel<TenantDto, TenantApiViewModel>(results);
            return Ok(new ApiResultModel<PaginationResult<TenantApiViewModel>>
            {
                Data = vmResults
            });
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResultModel<TenantApiViewModel>), 200)]
        public IActionResult Find(string id)
        {
            var result = _tr.Get(id);
            var vmResult = _mapper.Map<TenantDto, TenantApiViewModel>(result);
            return Ok(new ApiResultModel<TenantApiViewModel>
            {
                Data = vmResult
            });
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResultModel<TenantApiViewModel>), 200)]
        public IActionResult Create([FromBody] TenantApiViewModel vm)
        {
            var dto = _mapper.Map<TenantDto>(vm);
            dto = _tr.Create(dto);
            vm = _mapper.Map<TenantApiViewModel>(dto);
            return Ok(new ApiResultModel<TenantApiViewModel>
            {
                Data = vm
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResultModel<TenantApiViewModel>), 200)]
        public IActionResult Update([FromBody] TenantApiViewModel vm)
        {
            var dto = _mapper.Map<TenantDto>(vm);
            dto = _tr.Update(dto);
            vm = _mapper.Map<TenantApiViewModel>(dto);
            return Ok(new ApiResultModel<TenantApiViewModel>
            {
                Data = vm
            });
        }

        #region service mgmt

        [Route("{tenantId}/service/")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResultModel<IEnumerable<TenantServiceApiViewModel>>), 200)]
        public IActionResult GetServices(string tenantId)
        {
            var result = _tsr.GetServices4Tenant(tenantId);
            var vmResult = _mapper.Map<IEnumerable<TenantServiceApiViewModel>>(result);
            return Ok(new ApiResultModel<IEnumerable<TenantServiceApiViewModel>>
            {
                Data = vmResult
            });
        }

        [Route("{tenantId}/service/")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResultModel<TenantServiceApiViewModel>), 200)]
        public IActionResult UpdateServices([FromBody] TenantServiceApiViewModel vm)
        {
            var dto = _mapper.Map<TenantServiceDto>(vm);
            if (string.IsNullOrEmpty(dto.Id))
            {
                dto = _tsr.Create(dto);
            }
            else
            {
                dto = _tsr.Update(dto);
            }

            vm = _mapper.Map<TenantServiceApiViewModel>(dto);
            return Ok(new ApiResultModel<TenantServiceApiViewModel>
            {
                Data = vm
            });
        }

        #endregion
    }
}