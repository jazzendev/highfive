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
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }

    public class PermissionPolicyHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       PermissionRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "Permissions"))
            {
                return Task.CompletedTask;
            }

            var permissionClaims = context.User.FindAll("Permissions");
            var isAllow = false;

            if (permissionClaims.Count() != 0)
            {
                isAllow = permissionClaims.Any(c => c.Value == requirement.Permission);
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
