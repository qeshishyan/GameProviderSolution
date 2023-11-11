namespace CrashGameService.Service.Models
{
    public class BetListResponse
    {
        public int SessionId { get; set; }
        public List<UserBetResponse>? Bets { get; set; }
    }
}
