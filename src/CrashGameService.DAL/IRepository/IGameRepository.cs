using CrashGameService.Repository.Entities;

namespace CrashGameService.DAL.IRepository
{
    public interface IGameRepository
    {
        Task AddBetAsync(Bet bet);
        Task AddCashOutAsync(CashOut cashOut);
        Task AddRoundAsync(GameRound round);
        Task<int> AddSessionAsync(GameSession gameSession);
        Task<Bet> GetBetWithRoundAsync(int betId);
        Task<List<Bet>> GetLastBetsAsync(int count, int sessionId, int maxCount);
        Task<List<double>> GetLastMultipliersAsync(int count, int sessionId);
        Task<GameRound> GetRoundWithSessionAsync(int roundId);
        Task UpdateGameSessionAsync(GameSession session);
        Task UpdateRoundAsync(GameRound round);
    }
}