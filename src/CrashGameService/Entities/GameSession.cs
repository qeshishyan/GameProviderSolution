namespace CrashGameService.Entities
{
    public class GameSession
    {
        public int Id { get; set; }
        public string ProviderId { get; set; }
        public bool BettingTime { get; set; } = false;
        public List<GameRound> GameRounds { get; set; } = new List<GameRound> { };
        public GameRound? CurrentRound { get; set; } = null;
    }
}
