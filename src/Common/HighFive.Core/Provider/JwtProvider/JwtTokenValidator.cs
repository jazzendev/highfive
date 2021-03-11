using HighFive.Core.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HighFive.Core.Provider
{
    /// <summary>
    /// https://github.com/aspnet/Security/blob/master/src/Microsoft.AspNetCore.Authentication.JwtBearer/JwtBearerHandler.cs
    /// </summary>
    public class JwtTokenValidator : JwtSecurityTokenHandler
    {
        private readonly TokenConfig _config;

        public JwtTokenValidator(TokenConfig config)
        {
            _config = config;
        }

        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            switch (_config.EncryptMethod)
            {
                case EncryptMethod.SHA256:
                    return ValidateTokenSHA256(token, out validatedToken);
                case EncryptMethod.RSA256:
                    return ValidateTokenRSA256(token, out validatedToken);
                default:
                    return base.ValidateToken(token, validationParameters, out validatedToken);
            }
        }


        private ClaimsPrincipal ValidateTokenSHA256(string token, out SecurityToken validatedToken)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecurityKey));

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config.Issuer,
                ValidAudience = _config.Audience,
                IssuerSigningKey = key
            };

            return base.ValidateToken(token, parameters, out validatedToken);
        }


        private ClaimsPrincipal ValidateTokenRSA256(string token, out SecurityToken validatedToken)
        {
            using RSA rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(_config.RsaPublicKey), out _);
            var key = new RsaSecurityKey(rsa)
            {
                //prevent error
                //System.ObjectDisposedException: Cannot access a disposed object.
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config.Issuer,
                ValidAudience = _config.Audience,
                IssuerSigningKey = key
            };


            return base.ValidateToken(token, parameters, out validatedToken);
        }
    }
}
