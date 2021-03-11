using AutoMapper;
using AutoMapper.Configuration;
using HighFive.Core.Configuration;
using HighFive.Core.DomainModel;
using HighFive.Core.Model;
using HighFive.Core.Utility;
using HighFive.Domain.DomainModel;
using HighFive.Domain.Repository;
using HighFive.Web.Core;
using HighFive.Web.Core.Models;
using HighFive.Web.Core.Policies;
using HighFive.Web.Portal.ApiModels;
using HighFive.Web.Portal.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;

namespace HighFive.Web.Portal.ApiControllers
{
    [ApiController]
    [Route("api/error")]
    public class ErrorController : ControllerBase
    {

        [Route("{code}")]
        public IActionResult Error(int code, [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                return Problem();
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (code < 500)
            {
                var httpCode = (HttpStatusCode)code;
                return StatusCode(code, new ApiResultModel<string>
                {
                    Message = httpCode.ToString(),
                    Error = new ApiError()
                    {
                        Code = code.ToString(),
                        Message = httpCode.ToString()
                    }
                });
            }
            else
            {
                return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
            }
        }
    }
}