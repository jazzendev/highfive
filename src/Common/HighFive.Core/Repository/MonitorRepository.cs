using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Extensions.Logging;
using HighFive.Core.Model;

namespace HighFive.Core.Repository
{
    public class MonitorRepository : DatabaseRepository
    {
        private ClaimsPrincipal _principal;

        protected string CurrentUserId
        {
            get
            {
                string userId = string.Empty;

                if (_principal != null)
                {
                    if (_principal.FindFirst(ClaimTypes.NameIdentifier) != null)
                    {
                        userId = _principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                    }
                    else if (_principal.FindFirst("id") != null)
                    {
                        userId = _principal.FindFirst("id").Value;
                    }
                    else if (_principal.FindFirst("Id") != null)
                    {
                        userId = _principal.FindFirst("Id").Value;
                    }
                }

                if (string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentNullException(userId, "principal for current userId not found.");
                }

                return userId;
            }
        }

        protected string CurrentTenantId
        {
            get
            {
                string tenantId = string.Empty;

                if (_principal != null && _principal.FindFirst("TenantId") != null)
                {
                    tenantId = _principal.FindFirst("TenantId").Value;
                }

                if (string.IsNullOrEmpty(tenantId))
                {
                    throw new ArgumentNullException(tenantId, "principal for current tenantId not found.");
                }

                return tenantId;
            }
        }

        protected AppAuthLevel CurrentAuthLevel
        {
            get
            {
                AppAuthLevel authLevel = AppAuthLevel.NA;

                if (_principal != null && _principal.FindFirst("AuthLevel") != null)
                {
                    authLevel = (AppAuthLevel)Enum.Parse(typeof(AppAuthLevel), _principal.FindFirst("AuthLevel").Value);
                }

                return authLevel;
            }
        }

        public MonitorRepository(IConnectionFactory connectionFactory, ILogger logger, IPrincipal principal)
           : base(connectionFactory, logger)
        {
            _principal = principal as ClaimsPrincipal;
        }

        protected R AppendMonitorData<R>(R entity, DateTime? timestamp = null) where R : class
        {
            var imonitor = typeof(R).GetInterface("IMonitorModel");
            if (imonitor == null || string.IsNullOrEmpty(CurrentUserId))
            {
                return entity;
            }

            var obj = IMonitorExtension.AppendMonitorData(entity as IMonitorModel, CurrentUserId, timestamp) as R;
            return obj;
        }

        public void SetPrincipal(IPrincipal principal)
        {
            _principal = principal as ClaimsPrincipal;
        }
    }
}
