using Newtonsoft.Json;

namespace CrashGameService.Service.HttpClients
{
    public class GameLogicPhytonClient : IGameLogicPhytonClient
    {
        private readonly HttpClient _httpClient;
        public GameLogicPhytonClient(HttpClient httpClient)
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
