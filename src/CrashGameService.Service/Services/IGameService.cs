using CrashGameService.Service.Models;

namespace CrashGameService.Service.Services
{
    public interface IGameService
    {
        ValueTask<BetResponse> Bet(BetRequest bet);
        ValueTask<CashOutResponse> CashOut(CashOutRequest request);
        ValueTask StartGame();
    }
}
