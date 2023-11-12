using GameProviderService.Service.Models;

namespace GameProviderService.Service
{
    public interface IProviderService
    {
        Task<LaunchResponse> LaunchGame(LaunchRequest request);
        Task<GetSessionResponse> GetSessionInfo(string token);
    }
}