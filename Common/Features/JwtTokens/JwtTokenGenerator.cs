using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common.Features.JwtTokens
{
    public abstract class JwtTokenGenerator<TFor>
    {
        protected abstract string Key { get;  }
        protected abstract Claim[] Claims(TFor value);
        protected abstract JwtSecurityToken Token(Claim[] claims, SigningCredentials credentials);

        public virtual string GenerateToken(TFor value)
        {
            var claims = Claims(value);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = Token(claims, credentials);
            
            
            if (token == null)
                throw new InvalidOperationException("Token cannot be null.");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
