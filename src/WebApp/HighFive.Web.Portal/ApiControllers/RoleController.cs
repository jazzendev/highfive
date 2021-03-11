using AutoMapper;
using AutoMapper.Configuration;
using HighFive.Core.Configuration;
using HighFive.Core.DomainModel;
using HighFive.Domain.DomainModel;
using HighFive.Domain.Repository;
using HighFive.Web.Core;
using HighFive.Web.Core.Models;
using HighFive.Web.Portal.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;

namespace HighFive.Web.Portal.ApiControllers
{
    public class RoleController : AuthApiControllerBase
    {
        private readonly ILogger _logger;
        private readonly IRoleRepository _rr;
        private readonly IPermissionRepository _pr;
        private readonly IRolePermissionRepository _rpr;
        private readonly IAccountRepository _ar;

        public RoleController(
            IRoleRepository roleMgmtRepository,
            IPermissionRepository permissionRepository,
            IRolePermissionRepository rolePermissionRepository,
            IOptions<ApiDefaultValueConfig> config,
            ILogger<RoleController> logger,
            IMapper mapper) : base(logger, mapper)
        {
            _rr = roleMgmtRepository;
            _pr = permissionRepository;
            _rpr = rolePermissionRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResultModel<IEnumerable<RoleApiViewModel>>), 200)]
        public IActionResult GetRoles() {
            var result = _rr.GetAll();
            var vmResult = _mapper.Map<IEnumerable<RoleApiViewModel>>(result);
            return Ok(new ApiResultModel<IEnumerable<RoleApiViewModel>>
            {
                Data = vmResult
            });
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResultModel<RoleApiViewModel>), 200)]
        public IActionResult Find(string id)
        {
            var result = _rr.Get(id);
            var vmResult = _mapper.Map<RoleDto, RoleApiViewModel>(result);
            return Ok(new ApiResultModel<RoleApiViewModel>
            {
                Data = vmResult
            });
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResultModel<RoleApiViewModel>), 200)]
        public IActionResult Create([FromBody] RoleUpsertApiViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new ApiResultModel<string>
                {
                    Message = "字段验证失败。",
                    Error = new ApiError()
                    {
                        Code = "invalid",
                        Message = "字段验证失败。",
                        Field = "fields",
                        Resource = "role.create"
                    }
                });
            }

            var dto = _mapper.Map<RoleDto>(vm);
            dto = _rr.Create(dto);

            if (dto != null) {
                _rpr.SetRolePermissions(dto.Id, vm.Permissions);
            }

            var result = _mapper.Map<RoleApiViewModel>(dto);
            return Ok(new ApiResultModel<RoleApiViewModel>
            {
                Data = result
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResultModel<RoleApiViewModel>), 200)]
        public IActionResult Update([FromBody] RoleUpsertApiViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new ApiResultModel<string>
                {
                    Message = "字段验证失败。",
                    Error = new ApiError()
                    {
                        Code = "invalid",
                        Message = "字段验证失败。",
                        Field = "fields",
                        Resource = "role.update"
                    }
                });
            }

            var dto = _mapper.Map<RoleDto>(vm);
            dto = _rr.Update(dto);

            if (dto != null)
            {
                _rpr.SetRolePermissions(dto.Id, vm.Permissions);
            }

            var result = _mapper.Map<RoleApiViewModel>(dto);
            return Ok(new ApiResultModel<RoleApiViewModel>
            {
                Data = result
            });
        }

        [HttpGet]
        [Route("{roleId}/permission")]
        [ProducesResponseType(typeof(ApiResultModel<PermissionSelectionApiViewModel>), 200)]
        public IActionResult GetPermissions(string roleId)
        {
            if (roleId == "new") {
                roleId = string.Empty;
            }

            var candidateDtos = _pr.GetPermissionCandidates(roleId);
            var selectedDtos = _rpr.GetRolePermissions(roleId);

            var vm = new PermissionSelectionApiViewModel()
            {
                PermissionSelected = _mapper.Map<IEnumerable<SelectedPermissionApiViewModel>>(selectedDtos),
                PermissionCandidates = _mapper.Map<IEnumerable<PermissionApiViewModel>>(candidateDtos)
            };

            return Ok(new ApiResultModel<PermissionSelectionApiViewModel>
            {
                Data = vm
            });
        }
    }
}