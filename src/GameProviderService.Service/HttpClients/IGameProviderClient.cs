namespace GameProviderService.Service.HttpClients
{
    public interface IGameProviderClient
    {
        ValueTask<(double FirstOdd, double SecondOdd, double ThirdOdd)> GetOddsAsync();
    }
}
