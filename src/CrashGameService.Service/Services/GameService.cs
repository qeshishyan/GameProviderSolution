using AutoMapper;
using CrashGameService.DAL.IRepository;
using CrashGameService.Repository.Entities;
using CrashGameService.Service.HttpClients;
using CrashGameService.Service.Hubs;
using CrashGameService.Service.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Shared.Exceptions;

namespace CrashGameService.Service.Services
{
    public class GameService : IGameService
    {
        private readonly GameSession _currentGameSession;
        private readonly IHubContext<GameHub> _hubContext;
        private readonly IMapper _mapper;
        private readonly IGameRepository _repository;
        private readonly IGameLogicPhytonClient _client;

        private double brokenJet;

        public GameService(IHubContext<GameHub> hubContext,
            IMapper mapper, IGameRepository repository, IGameLogicPhytonClient client)
        {
            _currentGameSession = new GameSession
            {
                ProviderId = "providerId_4578d687ad65",
            };
            _hubContext = hubContext;
            _mapper = mapper;
            _repository = repository;
            _client = client;
        }

        public async ValueTask StartGame()
        {
            if (_currentGameSession.Started)
                throw new ApiException(400, "Game already started");

            _currentGameSession.Started = true;
            _currentGameSession.StartedDate = DateTime.UtcNow;

            await _repository.AddSessionAsync(_currentGameSession);

            await StartBettingTime();
        }

        public async ValueTask<BetResponse> Bet(BetRequest betRequest)
        {
            ValidateBet(betRequest);

            var bet = _mapper.Map<Bet>(betRequest);
            bet.BetDate = DateTime.UtcNow;
            bet.GameRound = _currentGameSession.CurrentRound;

            var saveTask = _repository.AddBetAsync(bet);

            //User id must be changed to real data
            object obj = new { Message = "BET", Value = bet.Value, User = "User_" + Guid.NewGuid().ToString() };
            var sendTask = SendToHub("BET", obj);

            await Task.WhenAll(saveTask, sendTask);

            // Save bet in db
            var response = new BetResponse { BetDate = bet.BetDate, GameRoundId = bet.GameRoundId, BetId = bet.Id };
            return response;
        }

        private void ValidateBet(BetRequest bet)
        {
            if (!_currentGameSession.Started)
                throw new ApiException(400, "Game not started");

            if (!_currentGameSession.CurrentRound.Id.Equals(bet.GameRoundId))
                throw new ApiException(400, "Invalid game round id");

            if (!_currentGameSession.BettingTime)
                throw new ApiException(400, "Betting time is closed");

            if (bet.Value <= 0)
                throw new ApiException(400, "Invalid bet value");

            if (bet.Multiplier > _currentGameSession.CurrentRound.Multiplier)
                throw new ApiException(400, "Invalid bet multiplier");
        }

        public async ValueTask<CashOutResponse> CashOut(CashOutRequest cashOutRequest)
        {
            if (cashOutRequest.Multiplier > _currentGameSession.CurrentRound.Multiplier)
                throw new ApiException(400, "Invalid multiplier");

            var cashOut = _mapper.Map<CashOut>(cashOutRequest);
            var task = _repository.AddCashOutAsync(cashOut);

            var bet = _currentGameSession.CurrentRound.Bets.Where(x => x.Id == cashOut.BetId).FirstOrDefault();
            if (bet is null)
                throw new ApiException(400, "Bet not found");

            bet.Win = true;

            await task;
            return new CashOutResponse { Value = bet.Value * cashOut.Multiplier };
        }

        private async ValueTask StartRound()
        {
            _currentGameSession.CurrentRound.Started = true;
            _currentGameSession.CurrentRound.StartDate = DateTime.UtcNow;
            var round = _currentGameSession.CurrentRound;

            var update = _repository.UpdateRoundAsync(round);

            //Send socket for round started

            var obj = new { Message = "Round started" };

            Task sendTask = SendToHub("RoundStarted", obj);
            await Task.WhenAll(update, sendTask);

            await PlayRound(round);
            await StartBettingTime();
        }

        private async Task PlayRound(GameRound round)
        {
            while (round.Multiplier < brokenJet)
            {
                round.Multiplier += 0.01d;
                var obj = new { Message = "Multiplier", Multiplier = round.Multiplier.ToString("F2") };
                await SendToHub("ReceiveMultiplier", obj);
                Thread.Sleep(20);
            }
            await CrashGame(round);
        }

        private async Task SendToHub(string method, object obj)
        {
            var multipJson = JsonConvert.SerializeObject(obj);
            await _hubContext.Clients.All.SendAsync(method, multipJson);
        }

        private async ValueTask CrashGame(GameRound round)
        {
            round.IsCrashed = true;
            round.EndDate = DateTime.UtcNow;

            var obj = new { Message = "Game crashed", Multiplier = round.Multiplier.ToString("F2") };
            Task sendTask = SendToHub("CrashGame", obj);
            
            Task update = _repository.UpdateRoundAsync(round);
            await Task.WhenAll(update, sendTask);
        }

        private async ValueTask CreateRound()
        {
            (double x1, double x2, double x3) = await _client.GetOddsAsync();
            Console.WriteLine("x1 " + x1);
            Console.WriteLine("x2 " + x2);
            Console.WriteLine("x3 " + x3);

            var array = new[] { x1, x2, x3 };
            brokenJet = array.Max();

            _currentGameSession.CurrentRound = new()
            {
                GameSessionId = _currentGameSession.Id,
                CreatedDate = DateTime.UtcNow
            };
            await _repository.AddRoundAsync(_currentGameSession.CurrentRound);
        }


        private async ValueTask StartBettingTime()
        {
            await CreateRound();
            Timer _timer = new(StopBettingTimeCallback, null, 5000, Timeout.Infinite);

            var obj = new { Message = "Betting time started", CurrentRoundId = _currentGameSession.CurrentRound.Id, TimeLeft = 5 };
            Task sendTask = SendToHub("StartBettingTime", obj);

            ValueTask betTimeTask = OnBettingTime();
            await Task.WhenAll(sendTask, betTimeTask.AsTask());
        }

        private async ValueTask OnBettingTime()
        {
            _currentGameSession.BettingTime = true;
            await _repository.UpdateGameSessionAsync(_currentGameSession);
        }
        private async ValueTask OffBettingTime()
        {
            _currentGameSession.BettingTime = false;
            await _repository.UpdateGameSessionAsync(_currentGameSession);
        }

        private async void StopBettingTimeCallback(object? state)
        {
            var obj = new { Message = "Betting time stoped" };
            Task saveTask = SendToHub("StopBettingTime", obj);
            ValueTask offTask = OffBettingTime();

            await Task.WhenAll(saveTask, offTask.AsTask());
            await StartRound();
        }
    }
}
