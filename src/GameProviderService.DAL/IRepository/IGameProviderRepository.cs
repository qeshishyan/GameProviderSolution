using GameProvider.Repository.Entities;

namespace GameProviderService.DAL.IRepository
{
    public interface IGameProviderRepository
    {
        Task<Merchant> GetMerchantAsync(string? merchantId);
    }
}