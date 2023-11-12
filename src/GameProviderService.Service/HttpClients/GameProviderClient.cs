using Newtonsoft.Json;

namespace GameProviderService.Service.HttpClients
{
    public class GameProviderClient : IGameProviderClient
    {
        private readonly HttpClient _httpClient;
        public GameProviderClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async ValueTask<(double FirstOdd, double SecondOdd, double ThirdOdd)> GetOddsAsync()
        {
            var response = await _httpClient.GetAsync("/getOdds");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var odds = JsonConvert.DeserializeObject<dynamic>(json);
                return ((double)odds.first_odd, (double)odds.second_odd, (double)odds.third_odd);
            }
            else
            {
                throw new Exception($"Failed to get odds: {response.StatusCode}");
            }
        }
    }
}
