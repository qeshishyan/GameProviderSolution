namespace CrashGameService.Entities
{
    public class GameRound
    {
        public int Id { get; set; }
        public int GameSessionId { get; set; }
        public double Multiplier { get; set; } = 1.00;
        public bool IsCrashed { get; set; } = false;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Bet> Bets { get; set; } = new List<Bet> { };
    }
}
