using CrashGameService.Entities;
using CrashGameService.Models;

namespace CrashGameService.Services
{
    public interface IGameService
    {
        Task<BetResponse> Bet(Bet bet);
        Task<double> CashOut(int betId, double multiplier);
        Task StartGame();
    }
}
