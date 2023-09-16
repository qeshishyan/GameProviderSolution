using CrashGameService.Models;

namespace CrashGameService.Services
{
    public interface IGameService
    {
        ValueTask<BetResponse> Bet(BetRequest bet);
        ValueTask<CashOutResponse> CashOut(CashOutRequest request);
        ValueTask StartGame();
    }
}
