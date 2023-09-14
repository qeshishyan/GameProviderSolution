using CrashGameService.Entities;
using CrashGameService.Models;

namespace CrashGameService.Services
{
    public interface IGameService
    {
        Task<BetResponse> Bet(BetRequest bet);
        Task<CashOutResponse> CashOut(CashOutRequest request);
        Task StartGame();
    }
}
