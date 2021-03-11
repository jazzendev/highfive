using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace HighFive.Web.Core
{
    [Authorize]
    public class BasePortalController : BaseController
    {
        private readonly ILogger _logger;

        protected string UserId { get { return CurrentUserId(); } }

        public BasePortalController(ILogger logger) : base(logger)
        {
            _logger = logger;
        }

        protected string CurrentUserId()
        {
            string userId = string.Empty;

            if (User != null)
            {
                if (User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                }
                else if (User.FindFirst("id") != null)
                {
                    userId = User.FindFirst("id").Value;
                }
                else if (User.FindFirst("Id") != null)
                {
                    userId = User.FindFirst("Id").Value;
                }
            }
            return userId;
        }
    }
}
