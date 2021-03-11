using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace HighFive.Core.Provider
{
    public interface IJwtProvider
    {
        string GenerateToken(string userId, string tenantId);
        string GenerateToken(Claim[] claims);
        bool ValidateToken(string token);
    }
}
