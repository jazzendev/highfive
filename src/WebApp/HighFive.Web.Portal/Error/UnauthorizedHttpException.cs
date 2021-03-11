using HighFive.Web.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HighFive.Web.Portal.Error
{
    public class UnauthorizedHttpException : Exception
    {
    }

    public class UnauthorizedHttpExceptionFilter : IActionFilter, IOrderedFilter
    {
        const string DEFAULT_ERROR_MESSAGE = "无访问权限";

        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is UnauthorizedHttpException exception)
            {
                var message = string.IsNullOrEmpty(exception.Message) ? DEFAULT_ERROR_MESSAGE : exception.Message;
                var result = new ObjectResult(new ApiResultModel<HttpStatusCode>
                {
                    Data = HttpStatusCode.Unauthorized,
                    Message = message,
                    Error = new ApiError()
                    {
                        Code = "invalid",
                        Message = message
                    }
                });
                result.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.ExceptionHandled = true;
            }
        }
    }
}
