using CSharpNewAPI.Interfaces;
using CSharpNewAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNewAPI.Services
{
    public class TokenService : ITokenService
    {

        public SymmetricSecurityKey SecurityKey { get; set; }
        public TokenService(IConfiguration configuration)
        {
            string secretSignature = configuration.GetValue<string>("TokenKey");
            SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretSignature));
        }

        public string GenerateToken(AppUser user)
        {

            DateTimeOffset date = DateTimeOffset.UtcNow.AddDays(3);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("username", user.UserName),
                }),
                Expires = date.UtcDateTime,

                SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(stoken);
            return token;
        }

        public bool validateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(SecurityKey.Key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
