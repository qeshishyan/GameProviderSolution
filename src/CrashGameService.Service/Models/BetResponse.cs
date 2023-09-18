namespace CrashGameService.Service.Models
{
    public class BetResponse
    {
        public int BetId { get; set; }
        public int GameRoundId { get; set; }
        public DateTimeOffset BetDate { get; set; }

    }
}
