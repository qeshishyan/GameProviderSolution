using AutoMapper;
using GameProviderService.DAL.IRepository;
using GameProviderService.Service.Models;
using Shared.Exceptions;

namespace GameProviderService.Service.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IMapper _mapper;
        private readonly IGameProviderRepository _repository;
        private readonly IJwtService _jwtService;
        public ProviderService(IMapper mapper,
            IGameProviderRepository repository, IJwtService jwtService)
        {
            _mapper = mapper;
            _repository = repository;
            _jwtService = jwtService;
        }

        public async Task<LaunchResponse> LaunchGame(LaunchRequest request)
        {
            try
            {
                ValidateLaunchGame(request);
                
                var merchant = await _repository.GetMerchantAsync(request.MerchantId);
                string token = _jwtService.GenerateToken(merchant.MerchantId!, request.Token!, merchant.Url!);

                var response = new LaunchResponse
                {
                    Token = token
                };
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ValidateLaunchGame(LaunchRequest request)
        {
            if (string.IsNullOrEmpty(request.MerchantId))
                throw new ApiException(400, "Invalid merchant id");

            if (string.IsNullOrEmpty(request.Token))
                throw new ApiException(400, "Invalid token");
        }
    }
}
