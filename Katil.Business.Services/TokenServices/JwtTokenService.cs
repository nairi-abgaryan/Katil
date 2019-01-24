using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Katil.Common.Utilities;
using Katil.Data.Model;
using Microsoft.IdentityModel.Tokens;

namespace Katil.Business.Services.TokenServices
{
    public class JwtTokenService : IJwtTokenService
    {
        public string GetJwtToken(User user)
        {
            const string signature = SettingKeys.HashKey;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var jsonToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(jsonToken);

            return token;
        }
    }
}