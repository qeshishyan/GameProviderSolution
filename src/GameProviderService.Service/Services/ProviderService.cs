using AutoMapper;
using GameProviderService.DAL.IRepository;
using GameProviderService.Service.HttpClients;
using GameProviderService.Service.Models;
using Shared.Exceptions;
using System.Security.Claims;

namespace GameProviderService.Service.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IMapper _mapper;
        private readonly IGameProviderRepository _repository;
        private readonly IJwtService _jwtService;
        private readonly IGameProviderClient _client;
        public ProviderService(IMapper mapper,
            IGameProviderRepository repository, IJwtService jwtService, IGameProviderClient client)
        {
            _mapper = mapper;
            _repository = repository;
            _jwtService = jwtService;
            _client = client;
        }

        public async Task<GetSessionResponse> GetSessionInfo(string token)
        {
            try
            {
                ClaimsPrincipal? credentials = _jwtService.ValidateToken(token);
                ValidateCredentials(credentials);

                var merchantIdClaim = credentials!.FindFirst("MerchantId")?.Value;
                var tokenClaim = credentials!.FindFirst("Token")?.Value;
                var url = credentials!.FindFirst("Url")?.Value;

                var response = await _client.GetSession(tokenClaim, merchantIdClaim, url);
                if (response is null)
                    throw new ApiException(500, "Session is null");

                return new GetSessionResponse
                {
                    Currency = response.Currency,
                    PlayerName = response.Player
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ValidateCredentials(ClaimsPrincipal? credentials)
        {
            if (credentials is null)
                throw new ApiException(403, "UnAuthorized");
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
