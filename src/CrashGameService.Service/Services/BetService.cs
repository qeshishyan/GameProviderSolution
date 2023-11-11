using AutoMapper;
using CrashGameService.DAL.IRepository;
using CrashGameService.Repository.Entities;
using CrashGameService.Service.Models;
using Shared.Exceptions;

namespace CrashGameService.Service.Services
{
    public class BetService : IBetService
    {
        private readonly IMapper _mapper;
        private readonly IGameRepository _repository;
        private readonly IGameHubService _hubService;

        public BetService(IMapper mapper, 
            IGameRepository repository, IGameHubService hubService)
        {
            _mapper = mapper;
            _repository = repository;
            _hubService = hubService;
        }

        public async ValueTask<BetResponse> Bet(BetRequest betRequest)
        {
            ValidateToken(betRequest.Token);

            var bet = _mapper.Map<Bet>(betRequest);
            bet.BetDate = DateTime.UtcNow;
            var curRound = await _repository.GetRoundWithSessionAsync(bet.GameRoundId);
            ValidateBet(betRequest, curRound);
            
            bet.GameRound = curRound;
            bet.Token = betRequest.Token;

            var saveTask = _repository.AddBetAsync(bet);

            //User id must be changed to real data
            object obj = new { Message = "BET", Value = bet.Value, User = "User_" + Guid.NewGuid().ToString() };
            var sendTask = _hubService.SendToHub("BET", obj);

            await Task.WhenAll(saveTask, sendTask);

            // Save bet in db
            var response = new BetResponse { BetDate = bet.BetDate, GameRoundId = bet.GameRoundId, BetId = bet.Id };
            return response;
        }

        private bool ValidateToken(string? token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ApiException(403, "UnAuthorized");

            return true;
        }

        private void ValidateBet(BetRequest bet, GameRound round)
        {
            if (!round.GameSession.Started)
                throw new ApiException(400, "Game session not started");

            if (round.IsCrashed || !round.Started)
                throw new ApiException(400, "Game round is not started or crashed");

            if (!round.GameSession.BettingTime)
                throw new ApiException(400, "Betting time is closed");

            if (bet.Value <= 0)
                throw new ApiException(400, "Invalid bet value");
        }

        public async ValueTask<CashOutResponse> CashOut(CashOutRequest cashOutRequest)
        {
            var bet = await _repository.GetBetWithRoundAsync(cashOutRequest.BetId);
            ValidateCashOut(cashOutRequest, bet);

            var cashOut = _mapper.Map<CashOut>(cashOutRequest);
            var task = _repository.AddCashOutAsync(cashOut);

            bet.Win = true;

            await task;
            return new CashOutResponse { Value = bet.Value * cashOut.Multiplier };
        }

        private static void ValidateCashOut(CashOutRequest cashOutRequest, Bet? bet)
        {
            if (bet is null)
                throw new ApiException(400, "Bet not found");

            if (cashOutRequest.Multiplier > bet.GameRound.Multiplier)
                throw new ApiException(400, "Invalid multiplier");
        }
    }
}
