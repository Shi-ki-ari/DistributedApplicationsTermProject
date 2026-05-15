using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StatusPageServices.Services
{
    public class TokenService
    {
        public string CreateSimpleToken(string username)
        {

            var claims = new[] { new Claim(ClaimTypes.Name, username) };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mnogosigurnaparola123456!@!@!@!@"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "StatusPageAPI",
                audience: "StatusPageClient",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2), 
                signingCredentials: creds
            );

            // 4. Turn it into a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}