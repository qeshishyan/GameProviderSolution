using System.Security.Claims;

namespace GameProviderService.Service.Services
{
    public interface IJwtService
    {
        string GenerateToken(string merchantId, string tokenValue, string url);
        ClaimsPrincipal? ValidateToken(string token);
    }
}