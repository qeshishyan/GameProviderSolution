using GameProviderService.Service.DTO;
using GameProviderService.Service.Services;
using Newtonsoft.Json;

namespace GameProviderService.Service.HttpClients
{
    public class GameProviderClient : IGameProviderClient
    {
        private readonly HttpClient _httpClient;
        private readonly ISignatureService _signature;
        public GameProviderClient(HttpClient httpClient, ISignatureService signature)
        {
            _httpClient = httpClient;
            _signature = signature;
        }

        public async ValueTask<SessionInfoResponseDTO?> GetSession(string token, string merchantId, string url)
        {
            try
            {
                _httpClient.BaseAddress = new Uri(url);
                string sing = _signature.GenerateSign(new Dictionary<string, string> { { "Token", token } }, merchantId);
                var response = await _httpClient.GetAsync($"/session?token={token}&sign={sing}");

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SessionInfoResponseDTO>(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
