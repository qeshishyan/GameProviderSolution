namespace CrashGameService.Service.Models
{
    public class MultiplierListResponse
    {
        public int SessionId { get; set; }
        public List<double>? Multipliers { get; set; }
    }
}
