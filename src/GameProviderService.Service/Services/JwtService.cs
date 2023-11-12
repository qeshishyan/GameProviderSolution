using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameProviderService.Service.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string merchantId, string tokenValue, string url)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecret"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("MerchantId", merchantId),
                new Claim("Token", tokenValue),
                new Claim("Url", url)
            };

            var jwtToken = new JwtSecurityToken(
                //issuer: "YourIssuer", // Optional
                //audience: "YourAudience", // Optional
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token expiry time
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecret"]!));
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                return tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            }
            catch
            {
                return null; // Token is not valid
            }
        }
    }
}
