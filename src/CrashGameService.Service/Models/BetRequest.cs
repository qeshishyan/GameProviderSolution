namespace CrashGameService.Service.Models
{
    public class BetRequest
    {
        public double Value { get; set; }
        public int GameRoundId { get; set; }
        public string? Token { get; set; }
    }
}
