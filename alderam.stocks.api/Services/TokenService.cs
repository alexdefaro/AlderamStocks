using alderam.stocks.api.Database;
using alderam.stocks.api.Models;
using alderam.stocks.api.Models.DTOs;
using AutoMapper;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace alderam.stocks.api.Services
{
    public interface ITokenService
    {
        string GenerateJWTToken(string userName);
    }

    public class TokenService : ITokenService
    {
        private IConfiguration _configuration { get; }

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJWTToken(string userName)
        {
            var jwtPrivateKey = Encoding.ASCII.GetBytes(_configuration["Secrets:jwt_private_key"]);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, userName),
                        new Claim(ClaimTypes.Role, "Investidor")
                    }
                ),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(jwtPrivateKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);

            var result = tokenHandler.WriteToken(jwtToken);

            return result;
        }
    }
}