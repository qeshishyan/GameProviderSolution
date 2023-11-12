using GameProviderService.Service.DTO;

namespace GameProviderService.Service.HttpClients
{
    public interface IGameProviderClient
    {
        ValueTask<SessionInfoResponseDTO?> GetSession(string token, string merchantId, string url);
    }
}
