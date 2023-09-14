using CrashGameService.Entities;
using CrashGameService.Models;

namespace CrashGameService.Services
{
    public interface IGameService
    {
        Task<BetResponse> Bet(Bet bet);
        Task<CashOutResponse> CashOut(CashOut request);
        Task StartGame();
    }
}
