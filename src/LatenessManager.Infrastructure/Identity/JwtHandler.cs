using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using LatenessManager.Application.Common.Models;
using LatenessManager.Application.Identity.Abstractions;
using LatenessManager.Infrastructure.Configurations;
using Microsoft.IdentityModel.Tokens;

namespace LatenessManager.Infrastructure.Identity
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtTokenConfiguration _jwtTokenConfiguration;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();
        private readonly JwtHeader _jwtHeader;

        public JwtHandler(JwtTokenConfiguration jwtTokenConfiguration)
        {
            _jwtTokenConfiguration = jwtTokenConfiguration;
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenConfiguration.Secret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            _jwtHeader = new JwtHeader(signingCredentials);
        }

        public JsonWebToken Create(int userId, string[] roles)
        {
            var nowUtc = DateTimeOffset.UtcNow;
            var expires = nowUtc.AddMinutes(_jwtTokenConfiguration.ExpiryMinutes);
            var exp = expires.ToUnixTimeSeconds();
            var iat = nowUtc.ToUnixTimeSeconds();
            var payload = new JwtPayload
            {
                {JwtRegisteredClaimNames.Sub, userId},
                {JwtRegisteredClaimNames.Iat, iat},
                {JwtRegisteredClaimNames.Exp, exp},
                {"roles", roles}
            };
            var jwt = new JwtSecurityToken(_jwtHeader, payload);
            var token = _jwtSecurityTokenHandler.WriteToken(jwt);

            return new JsonWebToken
            {
                AccessToken = token,
                Expires = exp
            };
        }
    }
}