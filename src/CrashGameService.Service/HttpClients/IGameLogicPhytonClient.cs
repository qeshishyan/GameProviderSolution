namespace CrashGameService.Service.HttpClients
{
    public interface IGameLogicPhytonClient
    {
        ValueTask<(double FirstOdd, double SecondOdd, double ThirdOdd)> GetOddsAsync();
    }
}
