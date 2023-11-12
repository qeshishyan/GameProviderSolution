namespace GameProvider.Repository.Entities
{
    public class Lobby
    {
        public int Id { get; set; }
        public string? Key { get; set; }
        public string? GameUrl { get; set; }
        public List<Merchant> Merchants { get; set; }
    }
}
