namespace GameProvider.Repository.Entities
{
    public class Merchant
    {
        public int Id { get; set; }
        public string? MerchantId { get; set; }
        public int LobbyId { get; set; }
        public Lobby Lobby { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Url { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
