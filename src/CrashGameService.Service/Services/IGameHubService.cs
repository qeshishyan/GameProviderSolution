namespace CrashGameService.Service.Services
{
    public interface IGameHubService
    {
        Task SendToHub(string method, object obj);
    }
}
