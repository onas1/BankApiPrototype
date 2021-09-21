using BankApi.Service.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankApi.Service.Implementation
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        public string GenerateToken(string userName, string UserId, string Email, IConfiguration Config)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName ),
                new Claim(ClaimTypes.Email, Email),
                new Claim(ClaimTypes.NameIdentifier, UserId)

            };
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.GetSection("JWT:Key").Value)),
                    SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.Now.AddHours(1)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenCreated = tokenHandler.CreateToken(securityTokenDescriptor);
            var token = tokenHandler.WriteToken(tokenCreated);

            return token;
        }
    }
}
