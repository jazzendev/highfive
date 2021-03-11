using AutoMapper;
using HighFive.Core.DomainModel;
using HighFive.Domain.DomainModel;
using HighFive.Domain.Repository;
using HighFive.Web.Core;
using HighFive.Web.Core.Models;
using HighFive.Web.Portal.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Security.Claims;
using System.Threading;
using HighFive.Core.Provider;
using System;
using HighFive.Core.Utility;

namespace HighFive.Web.Portal.ApiControllers
{
    [Route("api/auth")]
    public class AuthApiController : AuthApiControllerBase
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly IAccountRepository _ar;
        private readonly ILogger _logger;

        public AuthApiController(
            IJwtProvider jwtProvider,
            IAccountRepository accountRepository,
            ILogger<AuthApiController> logger,
            IMapper mapper) : base(logger, mapper)
        {
            _jwtProvider = jwtProvider;
            _ar = accountRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResultModel<UserApiViewModel>), 200)]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, new ApiResultModel<string>
                {
                    Message = "登录失败。",
                    Error = new ApiError()
                    {
                        Code = "invalid",
                        Message = "用户名或密码错误。",
                        Field = "PasswordHash",
                        Resource = "login"
                    }
                });
            }

            var password = request.PasswordHash.DecodeBase64();
            if (string.IsNullOrEmpty(password))
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, new ApiResultModel<string>
                {
                    Message = "登录失败。",
                    Error = new ApiError()
                    {
                        Code = "invalid",
                        Message = "用户名或密码错误。",
                        Field = "PasswordHash",
                        Resource = "login"
                    }
                });
            }

            var login = _ar.Verify(request.Username, password);
            if (login == null)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, new ApiResultModel<string>
                {
                    Message = "登录失败。",
                    Error = new ApiError()
                    {
                        Code = "invalid",
                        Message = "用户名或密码错误。",
                        Field = "PasswordHash",
                        Resource = "login"
                    }
                });
            }

            try
            {
                var user = _mapper.Map<UserApiViewModel>(login);
                user.Token = _jwtProvider.GenerateToken(login.Id, login.DefaultTenantId);
                return Ok(new ApiResultModel<UserApiViewModel>
                {
                    Data = user
                }); ;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogError(e.StackTrace);
                return StatusCode((int)HttpStatusCode.Unauthorized, new ApiResultModel<string>
                {
                    Message = "登录失败。",
                    Error = new ApiError()
                    {
                        Code = "invalid",
                        Message = "服务端错误。",
                        Field = "Server Error.",
                        Resource = "login"
                    }
                });
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetUserInfo()
        {
            var user = new UserApiViewModel()
            {
                Id = UserId,
                Name = "Jazzen Chen",
                Username = "Jazzen",
                TenantName = "test"
            };
            return Ok(new ApiResultModel<UserApiViewModel>
            {
                Data = user,
                Message = "登录成功。",
            });
        }



        [Route("Test")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Test()
        {
            return Ok(new ApiResultModel<string>
            {
                Data = "Success.",
                Message = "登录成功。",
            });
        }

        [AllowAnonymous]
        [Route("Base64")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResultModel<String>), 200)]
        public IActionResult Base64([FromQuery] string value)
        {
            var result = Convert.ToBase64String(Encoding.UTF8.GetBytes(value));

            return Ok(new ApiResultModel<string>
            {
                Data = result
            });
        }
    }
}