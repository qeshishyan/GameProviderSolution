using CrashGameService.Entities;

namespace CrashGameService.Services
{
    public interface IGameService
    {
        Task<Bet> Bet(Bet bet);
        Task<double> CashOut(int betId, double multiplier);
        Task StartGame();
    }
}
