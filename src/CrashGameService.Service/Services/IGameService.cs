using CrashGameService.Service.Models;

namespace CrashGameService.Service.Services
{
    public interface IGameService
    {
        ValueTask<BetListResponse> GetLastBets(int count, int sessionId);
        ValueTask<MultiplierListResponse> GetLastMultipliers(int count, int sessionId);
        ValueTask<GameSessionResponse> StartGame();
    }
}
