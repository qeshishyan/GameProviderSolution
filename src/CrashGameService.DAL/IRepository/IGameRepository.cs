using CrashGameService.Repository.Entities;

namespace CrashGameService.DAL.IRepository
{
    public interface IGameRepository
    {
        Task AddBetAsync(Bet bet);
        Task AddCashOutAsync(CashOut cashOut);
        Task AddRoundAsync(GameRound round);
        Task AddSessionAsync(GameSession gameSession);
        Task UpdateGameSessionAsync(GameSession session);
        Task UpdateRoundAsync(GameRound round);
    }
}