namespace CrashGameService.Entities
{
    public class GameSession
    {
        public int Id { get; set; }
        public int? CurrentRoundId { get; set; }
        public string? ProviderId { get; set; }
        public bool BettingTime { get; set; } = false;
        public bool Started { get; set; } = false;
        public DateTimeOffset StartedDate { get; set; }
        public virtual GameRound CurrentRound { get; set; }
        public virtual ICollection<GameRound> GameRounds { get; set; }
    }
}
