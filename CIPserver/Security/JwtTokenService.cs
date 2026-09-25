using DataBaseModule.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Security.Authentication
{
    public class JwtTokenService
    {
        private readonly string _secret;

        public JwtTokenService(string secret)
        {
            _secret = secret;
        }

        public string CreateToken(User user)
        {
            var claims = new[]
            {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(
                ClaimTypes.Name,
                user.Username)
        };

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _secret));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    claims: claims,
                    expires:
                        DateTime.UtcNow.AddHours(8),
                    signingCredentials:
                        credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
