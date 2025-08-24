using Common.Features.JwtTokens;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Modules.Users.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Users.Application.JWT
{
    public interface IUserJwt
    {
        string GenerateToken(User user);
    }
    public class UserJwt : JwtTokenGenerator<User>, IUserJwt
    {
        IConfiguration m_configuration;
        public UserJwt(IConfiguration configuration)
        {
            m_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        protected override string Key => m_configuration["Jwt:Key"] ?? throw new ConfigurationErrorsException("JWT Key is not configured.");

        protected override Claim[] Claims(User user)
        {
            var claims = new[]
            {           
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Accessibility),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
            };
            return claims;
        }

        protected override JwtSecurityToken Token(Claim[] claims, SigningCredentials credentials)
        {
            return new JwtSecurityToken(
                issuer: m_configuration["Jwt:Issuer"],
                audience: m_configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(m_configuration["Jwt:ExpiresInMinutes"]!)),
                signingCredentials: credentials
            );
        }
    }
}
