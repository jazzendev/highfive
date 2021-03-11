using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using HighFive.Core.Repository;
using HighFive.Web.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Cryptography;
using HighFive.Core.DomainModel;
using System.Net.Mime;
using HighFive.Core.Model;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace HighFive.Web.Core
{
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        private readonly ILogger _logger;
        protected readonly IMapper _mapper;

        public ApiControllerBase(ILogger logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        [HttpOptions("{id}")]
        public IActionResult PreflightRoute(int id)
        {
            return NoContent();
        }

        [HttpOptions]
        public IActionResult PreflightRoute()
        {
            return NoContent();
        }

        protected ApiResultModel<T> ValidateModel<T>(T model)
        {
            if (ModelState.IsValid)
            {
                return null;
            }

            var firstError = ModelState.Where(ms => ms.Value.Errors.Any())
                                       .Select(f => new { f.Key, f.Value.Errors })
                                       .FirstOrDefault();

            if (firstError == null)
            {
                return null;
            }

            var field = firstError.Key;
            var message = firstError.Errors.Select(e => e.ErrorMessage).FirstOrDefault();

            ApiResultModel<T> resultModel = new ApiResultModel<T>()
            {
                Data = model,
                Message = "验证失败。",
                Error = new ApiError()
                {
                    Code = "invalid",
                    Message = message,
                    Field = field,
                    Resource = null
                }
            };

            return resultModel;
        }

        public PaginationResult<R> MapPaginationModel<T, R>(PaginationResult<T> target)
        {
            var data = target.Data;
            var newData = _mapper.Map<IEnumerable<R>>(data);

            var newResult = new PaginationResult<R>()
            {
                Data = newData,
                //Query = target.Query,
                Page = target.Page,
                Size = target.Size,
                Total = target.Total
            };

            return newResult;
        }

        protected string TryGetUserId()
        {
            string userId = User == null ? null :
                User.FindFirst("Id") == null ? null :
                User.FindFirst("Id").Value;

            return userId;
        }

        protected string TryGetTenantId()
        {
            string tenantId = User == null ? null :
                User.FindFirst("TenantId") == null ? null :
                User.FindFirst("TenantId").Value;

            return tenantId;
        }

        protected AppAuthLevel TryGetAuthLevel()
        {
            AppAuthLevel authLevel = AppAuthLevel.NA;

            if (User != null && User.FindFirst("AuthLevel") != null)
            {
                authLevel = (AppAuthLevel)Enum.Parse(typeof(AppAuthLevel), User.FindFirst("AuthLevel").Value);
            }

            return authLevel;
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthApiControllerBase : ApiControllerBase
    {
        const string DEFAULT_ERROR_MESSAGE = "无访问权限";

        private readonly ILogger _logger;

        public AuthApiControllerBase(ILogger logger, IMapper mapper) : base(logger, mapper)
        {
            _logger = logger;
        }

        protected string UserId
        {
            get
            {
                var userId = TryGetUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentNullException(userId, "principal for current userId not found.");
                }
                return userId;
            }
        }

        protected string TenantId
        {
            get
            {
                var tenantId = TryGetTenantId();

                if (string.IsNullOrEmpty(tenantId))
                {
                    throw new ArgumentNullException(tenantId, "principal for current tenantId not found.");
                }
                return tenantId;
            }
        }

        protected AppAuthLevel AuthLevel
        {
            get
            {
                return TryGetAuthLevel();
            }
        }

        public new IActionResult Unauthorized()
        {
            return Unauthorized(string.Empty);
        }

        public IActionResult Unauthorized(string message = "")
        {
            message = string.IsNullOrEmpty(message) ? DEFAULT_ERROR_MESSAGE : message;
            var result = new ApiResultModel<HttpStatusCode>
            {
                Data = HttpStatusCode.Unauthorized,
                Message = message,
                Error = new ApiError()
                {
                    Code = StatusCodes.Status401Unauthorized.ToString(),
                    Message = message
                }
            };

            return StatusCode(StatusCodes.Status401Unauthorized, result);
        }
    }
}
