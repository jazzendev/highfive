using HighFive.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Web.Core.Policies
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public IEnumerable<Roles> Roles { get; private set; }

        public RoleRequirement(IEnumerable<Roles> roles)
        {
            Roles = roles;
        }
    }

    public class RolePolicyHandler : AuthorizationHandler<RoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       RoleRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                return Task.CompletedTask;
            }

            var roleString = context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value;
            var isAllow = false;

            if (!string.IsNullOrEmpty(roleString))
            {
                var roles = roleString.Split(',');

                foreach (var r in requirement.Roles)
                {
                    isAllow = roles.Contains(r.ToString());
                    if (isAllow)
                    {
                        break;
                    }
                }
            }

            if (isAllow)
            {
                context.Succeed(requirement);
            }

            //TODO: Use the following if targeting a version of
            //.NET Framework older than 4.6:
            //      return Task.FromResult(0);
            return Task.CompletedTask;
        }
    }
}
