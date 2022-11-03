using Bank_API.BusinessLogicLayer.Interfaces;
using Bank_API.DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Bank_API.BusinessLogicLayer.Services
{
    public  class TokenService : ITokenService
    {
        public readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
 
        public string GenerateAccessToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //var claims = new Claim("Id", user.Id.ToString()).ToString();
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        public string GenerateRefreshToken()
        {
            var random = new byte[64];

            using (var rand = RandomNumberGenerator.Create())
            {
                rand.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
    }
}
