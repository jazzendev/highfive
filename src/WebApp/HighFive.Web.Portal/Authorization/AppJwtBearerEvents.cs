using HighFive.Domain.Repository;
using HighFive.Web.Portal.Error;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HighFive.Web.Portal.Authorization
{
    public class AppJwtBearerEvents : JwtBearerEvents
    {
        private readonly IAppAccessRepository _ar;

        public AppJwtBearerEvents(IAppAccessRepository repository)
        {
            _ar = repository;
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            // Add the access_token as a claim, as we may actually need it
            var token = context.SecurityToken as JwtSecurityToken;

            var user = _ar.GetAppAccess(context.Principal.FindFirstValue("Id"), context.Principal.FindFirstValue("TenantId"));

            if (user == null)
            {
                context.Fail(new UnauthorizedHttpException());
            }
            else
            {

                //Add claim if they are
                var claims = new List<Claim>
                    {
                        // claims will be appended, no need to add again
                        //new Claim("Id", user.Id),
                        //new Claim("TenantId", user.DefaultTenantId),
                        new Claim("Name", user.Name),
                        new Claim("AuthLevel", user.Roles.Count() == 0 ? int.MaxValue.ToString():user.Roles.Min(r=>r.AuthLevel).ToString())
                    };

                claims.AddRange(user.Roles.Select(r => new Claim("Roles", r.Name.ToString())));
                claims.AddRange(user.Roles.SelectMany(r => r.Permissions).Distinct().Select(p => new Claim("Permissions", p.ToString())));
                claims.AddRange(user.Services.Select(s => new Claim("Services", s.ToString())));


                var appIdentity = new ClaimsIdentity(claims);

                context.Principal.AddIdentity(appIdentity);
            }
        }
    }
}
