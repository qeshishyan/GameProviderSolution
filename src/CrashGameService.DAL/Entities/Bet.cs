namespace CrashGameService.Repository.Entities
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
        public  GameRound GameRound { get; set; } 
    }
}
