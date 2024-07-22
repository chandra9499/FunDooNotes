using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model.Models.DTOs.Token;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLogicLayer.Helper
{
    public class TokenGenarator
    {
        private readonly IConfiguration _configuration;
        public TokenGenarator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public TokenResponce GetToken(IEnumerable<Claim> claim)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(7),
                claims: claim,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenResponce { TokenString = tokenString, ValidTo = token.ValidTo };
        }
        public string GetRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public IEnumerable<Claim> GetClaimsFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(_configuration["JWT:secret"]);

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false, // adjust as necessary
                    ValidateAudience = false, // adjust as necessary
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                return claimsPrincipal.Claims;
            }
            catch (Exception ex)
            {
                // Handle validation errors, if any
                throw new SecurityTokenException("Invalid token", ex);
            }
        }


    }
}
