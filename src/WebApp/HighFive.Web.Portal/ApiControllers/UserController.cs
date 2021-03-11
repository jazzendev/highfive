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
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HighFive.Web.Portal.ApiControllers
{
    public class UserController : AuthApiControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAccountRepository _ar;
        private readonly IRoleRepository _rr;

        public UserController(
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
        [ProducesResponseType(typeof(ApiResultModel<AccountApiViewModel>), 200)]
        public IActionResult Find()
        {
            var result = _ar.Get(UserId);
            var vm = _mapper.Map<AccountDetailApiViewModel>(result);
            return Ok(new ApiResultModel<AccountApiViewModel>
            {
                Data = vm
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResultModel<AccountApiViewModel>), 200)]
        public IActionResult Update([FromBody] AccountApiViewModel vm)
        {
            var result = _ar.Get(UserId);
            vm = _mapper.Map<AccountApiViewModel>(result);
            return Ok(new ApiResultModel<AccountApiViewModel>
            {
                Data = vm
            });
        }

        [Route("password")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResultModel<AccountApiViewModel>), 200)]
        public IActionResult ChangePassword([FromBody] ChangePasswordApiViewModel vm,
            [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            var result = _ar.UpdatePassword(UserId, vm.OldPassword.DecodeBase64(), vm.NewPassword.DecodeBase64());

            if (!result) {
                ModelState.AddModelError("OldPassword", "密码错误。");
                return apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }

            return Ok(new ApiResultModel<HttpStatusCode>
            {
                Data = HttpStatusCode.OK
            });
        }

        // Get the default form options so that we can use them to set the default limits for
        // request body data
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        private const string FILE_PATH = "content/avatars";

        //[HttpPost]
        //[DisableFormValueModelBinding]
        //[RequestSizeLimit(1_048_576)]
        //public async Task<IActionResult> Avatar()
        //{
        //    var result = new JsonResponse() { IsSuccess = false };

        //    if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
        //    {
        //        result.Messages.Add($"content-type 错误, 当前content-type为{Request.ContentType}");
        //        return Json(result);
        //    }

        //    var dto = _pr.GetPortalLogin(UserId);
        //    if (dto == null)
        //    {
        //        result.Messages.Add($"请求数据错误。Null dto。");
        //        return Json(result);
        //    }

        //    string url = string.Empty;

        //    try
        //    {
        //        var boundary = MultipartRequestHelper.GetBoundary(
        //            MediaTypeHeaderValue.Parse(Request.ContentType),
        //            _defaultFormOptions.MultipartBoundaryLengthLimit);
        //        var reader = new MultipartReader(boundary, HttpContext.Request.Body);

        //        var section = await reader.ReadNextSectionAsync();
        //        if (section != null)
        //        {
        //            ContentDispositionHeaderValue contentDisposition;
        //            var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

        //            if (hasContentDispositionHeader)
        //            {
        //                if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
        //                {
        //                    var name = contentDisposition.FileName.Value.Replace("\"", "").Replace("/", "\\");
        //                    var patterns = name.Split('\\').LastOrDefault().Split('.');
        //                    var extension = patterns.LastOrDefault().ToLower();
        //                    var localFileName = $"{dto.Id}{Path.GetExtension(name)}";

        //                    using (var stream = new MemoryStream())
        //                    {
        //                        await section.Body.CopyToAsync(stream);
        //                        stream.Position = 0;
        //                        url = await _storage.SaveFileAsync(localFileName, $"{FILE_PATH}", stream, stream.Length);
        //                        var nonceStr = IdHelper.NewId();
        //                        url = $"{url}?nonce={nonceStr.Substring(nonceStr.Length - 4)}";

        //                        dto = await _pr.UpdateAvatar(dto.Id, url);

        //                        if (dto != null)
        //                        {
        //                            result.Result = url;
        //                            result.IsSuccess = true;
        //                            return Json(result);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        result.Messages.Add(e.Message);

        //        _logger.LogError(e.Message);
        //        _logger.LogError(e.StackTrace);
        //    }

        //    return Json(result);
        //}
    }
}