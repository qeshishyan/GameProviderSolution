using GameProvider.Repository.Entities;
using GameProviderService.DAL.IRepository;
using GameProviderService.Repository.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

namespace GameProviderService.DAL.Repository
{
    public class GameProviderRepository : IGameProviderRepository
    {
        private readonly GameProviderDbContext _dbContext;
        public GameProviderRepository(GameProviderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Merchant> GetMerchantAsync(string? merchantId)
        {
            try
            {
                if(merchantId is null)
                    throw new ArgumentNullException(nameof(merchantId));

                var result = await _dbContext.Merchants.Where(x => x.MerchantId == merchantId)
                    .FirstOrDefaultAsync();

                if (result == null)
                    throw new ApiException(400, "Invalid merchant id");

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
