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
                var jsonRequest = JsonConvert.SerializeObject(new SessionInfoRequestDTO
                {
                    Sign = _signature.GenerateSign(new Dictionary<string, string> { { "Token", token } }, merchantId),
                    Token = token
                });
                var httpContent = new StringContent(jsonRequest);
                var response = await _httpClient.PostAsync("/getSession", httpContent);

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
