namespace CrashGameService.Entities
{
    public class GameSession
    {
        public int Id { get; set; }
        public string? ProviderId { get; set; }
        public bool BettingTime { get; set; } = false;
        public bool Started { get; set; } = false;
        public DateTime StartedDate { get; set; }
        public List<GameRound> GameRounds { get; set; } = new List<GameRound> { };
        public GameRound CurrentRound { get; set; } = new GameRound();
    }
}
