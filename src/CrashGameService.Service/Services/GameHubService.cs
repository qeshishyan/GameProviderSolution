using CrashGameService.Service.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace CrashGameService.Service.Services
{
    public class GameHubService : IGameHubService
    {
        private readonly IHubContext<GameHub> _hubContext;
        public GameHubService(IHubContext<GameHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToHub(string method, object obj)
        {
            var multipJson = JsonConvert.SerializeObject(obj);
            await _hubContext.Clients.All.SendAsync(method, multipJson);
        }
    }
}
