using AutoMapper;
using AutoMapper.Configuration;
using HighFive.Core.Configuration;
using HighFive.Core.DomainModel;
using HighFive.Core.Model;
using HighFive.Core.Utility;
using HighFive.Domain;
using HighFive.Domain.DomainModel;
using HighFive.Domain.Repository;
using HighFive.Web.Core;
using HighFive.Web.Core.Models;
using HighFive.Web.Core.Policies;
using HighFive.Web.Portal.ApiModels;
using HighFive.Web.Portal.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;

namespace HighFive.Web.Portal.ApiControllers
{
    [AppPermission(AppPermissionCode.Tenant_Account_Management)]
    public class AccountController : AuthApiControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAccountRepository _ar;
        private readonly IRoleRepository _rr;

        public AccountController(
            IAccountRepository accountRepository,
            IRoleRepository roleRepository,
            ILogger<AccountController> logger,
            IMapper mapper) : base(logger, mapper)
        {
            _ar = accountRepository;
            _rr = roleRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResultModel<PaginationResult<AccountApiViewModel>>), 200)]
        public IActionResult Query([FromQuery] AccountQuery query)
        {
            query.TenantId = TenantId;
            var results = _ar.Query(query);
            var vmResults = MapPaginationModel<AccountDisplayDto, AccountApiViewModel>(results);
            return Ok(new ApiResultModel<PaginationResult<AccountApiViewModel>>
            {
                Data = vmResults
            });
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResultModel<AccountDetailApiViewModel>), 200)]
        public IActionResult Find(string id)
        {
            var result = _ar.Get(id, TenantId);

            if (result != null && result.TenantId == TenantId)
            {
                var vmResult = _mapper.Map<AccountDisplayDto, AccountDetailApiViewModel>(result);
                return Ok(new ApiResultModel<AccountDetailApiViewModel>
                {
                    Data = vmResult
                });
            }

            return Unauthorized();
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResultModel<AccountDetailApiViewModel>), 200)]
        public IActionResult Create([FromBody] AccountDetailApiViewModel vm)
        {
            var dto = _mapper.Map<AccountDto>(vm);
            dto.TenantId = TenantId;
            dto = _ar.Create(dto);
            vm = _mapper.Map<AccountDetailApiViewModel>(dto);
            return Ok(new ApiResultModel<AccountDetailApiViewModel>
            {
                Data = vm
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResultModel<AccountDetailApiViewModel>), 200)]
        public IActionResult Update([FromBody] AccountDetailApiViewModel vm)
        {
            var dto = _mapper.Map<AccountDto>(vm);
            dto = _ar.Update(dto);
            vm = _mapper.Map<AccountDetailApiViewModel>(dto);
            return Ok(new ApiResultModel<AccountDetailApiViewModel>
            {
                Data = vm
            });
        }

        [Route("{accountId}/resetpassword")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResultModel<HttpStatusCode>), 200)]
        public IActionResult ResetPassword(string accountId)
        {
            var result = _ar.ResetPassword(accountId);

            return Ok(new ApiResultModel<HttpStatusCode>
            {
                Data = HttpStatusCode.OK
            });
        }

        [Route("{accountId}/roles")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResultModel<IEnumerable<AccountRoleApiViewModel>>), 200)]
        public IActionResult GetRoles(string accountId)
        {
            var dtos = _ar.GetRoles(accountId, TenantId);
            var vms = _mapper.Map<IEnumerable<AccountRoleApiViewModel>>(dtos);

            return Ok(new ApiResultModel<IEnumerable<AccountRoleApiViewModel>>
            {
                Data = vms
            });
        }

        //[Route("{accountId}/roles")]
        //[HttpPost]
        //[ProducesResponseType(typeof(ApiResultModel<IEnumerable<AccountRoleApiViewModel>>), 200)]
        //public IActionResult ChangeRoles(string accountId, [FromBody] AccountRoleUpdateApiViewModel vm)
        //{
        //    var dtos = _ar.SetRoles(accountId, TenantId, vm.RoleIds);
        //    var vms = _mapper.Map<IEnumerable<AccountRoleApiViewModel>>(dtos);

        //    return Ok(new ApiResultModel<IEnumerable<AccountRoleApiViewModel>>
        //    {
        //        Data = vms
        //    });
        //}
    }
}