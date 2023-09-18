namespace CrashGameService.Repository.Entities
{
    public class CashOut
    {
        public int Id { get; set; }
        public int BetId { get; set; }
        public string? UserId { get; set; }
        public double Multiplier { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public Bet Bet { get; set; }
    }
}
