namespace CrashGameService.Entities
{
    public class Bet
    {
        public int Id { get; set; }
        public int GameRoundId { get; set; }
        public double Value { get; set; }
        public DateTimeOffset BetDate { get; set; }
        public bool Win { get; set; } = false;
        public double Multiplier { get; set; }
        public string? Token { get; set; }
        public virtual GameRound GameRound { get; set; } 
        public virtual ICollection<CashOut> CashOuts { get; set; } 

    }
}
