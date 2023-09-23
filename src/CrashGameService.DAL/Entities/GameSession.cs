namespace CrashGameService.Repository.Entities
{
    public class GameSession
    {
        public int Id { get; set; }
        public int? CurrentRoundId { get; set; }
        public string? ProviderId { get; set; }
        public bool BettingTime { get; set; } = false;
        public bool Started { get; set; } = false;
        public DateTimeOffset StartedDate { get; set; }
        public  GameRound CurrentRound { get; set; }
        public  ICollection<GameRound> GameRounds { get; set; }
    }
}
