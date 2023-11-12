namespace GameProviderService.Service.Services
{
    public interface ISignatureService
    {
        string GenerateSign(IDictionary<string, string> parameters, string secretKey);
    }
}