namespace CrashGameService.Repository.Entities
{
    public class GameRound
    {
        public int Id { get; set; }
        public int GameSessionId { get; set; }
        public double Multiplier { get; set; } = 1.00;
        public bool IsCrashed { get; set; } = false;
        public bool Started { get; set; } = false;
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public GameSession GameSession { get; set; }
        public ICollection<Bet> Bets { get; set; }
    }
}
