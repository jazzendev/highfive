using HighFive.Core.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HighFive.Core.Provider
{
    public enum EncryptMethod
    {
        NA = 0,
        SHA256 = 1,
        RSA256 = 2
    }

    public class JwtProvider : IJwtProvider
    {
        private readonly TokenConfig _config;
        private JwtTokenValidator _validator;
        public JwtTokenValidator Validator { get => _validator; }

        public JwtProvider(TokenConfig config)
        {
            _config = config;
            _validator = new JwtTokenValidator(config);
        }

        public string GenerateToken(string userId, string tenantId)
        {
            var claims = new[]
            {
                new Claim("Id", userId),
                new Claim("TenantId", tenantId)
            };

            return GenerateToken(claims);
        }

        public string GenerateToken(Claim[] claims)
        {
            switch (_config.EncryptMethod)
            {
                case EncryptMethod.SHA256:
                    return GenerateTokenSHA256(claims);
                case EncryptMethod.RSA256:
                    // !IMPORTANT, TO MAKE RSA WORK, MUST CONFIG AZURE FIRST!
                    // https://stackoverflow.com/questions/46114264/x509certificate2-on-azure-app-services-azure-websites-since-mid-2017
                    return GenerateTokenRSA256(claims);
                default:
                    return string.Empty;
            }
        }

        public bool ValidateToken(string token)
        {
            try
            {
                _validator.ValidateToken(token, null, out var validatedSecurityToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GenerateTokenSHA256(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8).AddDays(7),
                signingCredentials: creds);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        /// <summary>
        /// https://vmsdurano.com/-net-core-3-1-signing-jwt-with-rsa/
        /// https://vcsjones.dev/2019/10/07/key-formats-dotnet-3/
        /// https://unix.stackexchange.com/questions/415970/openssl-genpkey-algorithm-rsa-vs-genrsa
        /// We use PKCS#8 here
        /// 
        /// !IMPORTANT, TO MAKE RSA WORK, MUST CONFIG AZURE FIRST!
        /// https://stackoverflow.com/questions/46114264/x509certificate2-on-azure-app-services-azure-websites-since-mid-2017
        /// https://stackoverflow.com/questions/18959418/site-in-azure-websites-fails-processing-of-x509certificate2
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private string GenerateTokenRSA256(Claim[] claims)
        {

            using RSA rsa = RSA.Create();
            rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(_config.RsaPrivateKey), out _);

            var creds = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
            {
                //prevent error
                //System.ObjectDisposedException: Cannot access a disposed object.
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };

            var now = DateTime.Now;
            var unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();

            var token = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8).AddDays(7),
                signingCredentials: creds
            );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
