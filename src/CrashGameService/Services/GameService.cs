using AutoMapper;
using CrashGameService.Context;
using CrashGameService.Entities;
using CrashGameService.Hubs;
using CrashGameService.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Shared.Exceptions;

namespace CrashGameService.Services
{
    public class GameService : IGameService
    {
        private readonly GameSession _currentGameSession;
        private readonly CancellationToken _token;
        private readonly IHubContext<GameHub> _hubContext;
        private readonly IMapper _mapper;
        private readonly CrashDbContext _dbContext;

        private double brokenJet;

        public GameService(IHubContext<GameHub> hubContext,
            IMapper mapper, CrashDbContext dbContext)
        {
            _currentGameSession = new GameSession
            {
                ProviderId = "providerId_4578d687ad65",
            };
            _token = new CancellationTokenSource().Token;
            _hubContext = hubContext;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async ValueTask StartGame()
        {
            if (_currentGameSession.Started)
                throw new ApiException(400, "Game already started");

            _currentGameSession.Started = true;
            _currentGameSession.StartedDate = DateTime.UtcNow;

            await _dbContext.GameSessions.AddAsync(_currentGameSession);
            await _dbContext.SaveChangesAsync();

            await StartBettingTime();
        }

        public async ValueTask<BetResponse> Bet(BetRequest betRequest)
        {
            ValidateBet(betRequest);

            var bet = _mapper.Map<Bet>(betRequest);
            bet.BetDate = DateTime.UtcNow;
            bet.GameRound = _currentGameSession.CurrentRound;

            await _dbContext.Bets.AddAsync(bet);
            var saveTask = _dbContext.SaveChangesAsync();

            //User id must be changed to real data
            object obj = new { Message = "BET", Value = bet.Value, User = "User_" + Guid.NewGuid().ToString() };
            var betJson = JsonConvert.SerializeObject(obj);
            var sendTask = _hubContext.Clients.All.SendAsync("BET", betJson);

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
            _dbContext.CashOuts.Add(cashOut);

            var bet = _currentGameSession.CurrentRound.Bets.Where(x => x.Id == cashOut.BetId).FirstOrDefault();
            if (bet is null)
                throw new ApiException(400, "Bet not found");

            bet.Win = true;

            return await Task.FromResult(new CashOutResponse { Value = bet.Value * cashOut.Multiplier });
        }

        private async ValueTask StartRound()
        {
            _currentGameSession.CurrentRound.Started = true;
            _currentGameSession.CurrentRound.StartDate = DateTime.UtcNow;
            var round = _currentGameSession.CurrentRound;

            _dbContext.Update<GameRound>(round);
            var saveTask = _dbContext.SaveChangesAsync();

            //Send socket for round started
            var json = JsonConvert.SerializeObject(new { Message = "Round started" });
            Task sendTask = _hubContext.Clients.All.SendAsync("StartRound", json);
            await Task.WhenAll(saveTask, sendTask);

            await PlayRound(round);
            await StartBettingTime();
        }

        private async Task PlayRound(GameRound round)
        {
            while (round.Multiplier < brokenJet)
            {
                round.Multiplier += 0.1d;
                var multipJson = JsonConvert.SerializeObject(new { Message = "Multiplier", Multiplier = round.Multiplier.ToString("F1") });
                await _hubContext.Clients.All.SendAsync("ReceiveMultiplier", multipJson);
                Thread.Sleep(200);
            }
            await CrashGame(round);
        }

        private async ValueTask CrashGame(GameRound round)
        {
            round.IsCrashed = true;
            round.EndDate = DateTime.UtcNow;

            var crashJson = JsonConvert.SerializeObject(new { Message = "Game crashed", Multiplier = round.Multiplier.ToString("F1") });
            Task sendTask = _hubContext.Clients.All.SendAsync("CrashGame", crashJson);

            _dbContext.Update<GameRound>(round);
            Task saveTask = _dbContext.SaveChangesAsync();

            await Task.WhenAll(saveTask, sendTask);
        }

        private async ValueTask CreateRound()
        {
            CrashGameLogic logic = new CrashGameLogic();
            (double x1, double x2, double x3) = logic.GenerateOdds();
            var array = new[] { x1, x2, x3 };
            brokenJet = array.Max();

            _currentGameSession.CurrentRound = new()
            {
                // Database key
                //Id = new Random().Next(10000, 99999),
                GameSessionId = _currentGameSession.Id,
                CreatedDate = DateTime.UtcNow
            };
            await _dbContext.GameRounds.AddAsync(_currentGameSession.CurrentRound);
            await _dbContext.SaveChangesAsync();
        }


        private async ValueTask StartBettingTime()
        {
            await CreateRound();
            Timer _timer = new(StopBettingTimeCallback, null, 5000, Timeout.Infinite);

            string crashJson = JsonConvert.SerializeObject(new { Message = "Betting time started", CurrentRoundId = _currentGameSession.CurrentRound.Id, TimeLeft = 5 });
            Task sendTask = _hubContext.Clients.All.SendAsync("StartBettingTime", crashJson);
            ValueTask betTimeTask = OnBettingTime();
            await Task.WhenAll(sendTask, betTimeTask.AsTask());
        }

        private async ValueTask OnBettingTime()
        {
            _currentGameSession.BettingTime = true;
            _dbContext.Update<GameSession>(_currentGameSession);
            await _dbContext.SaveChangesAsync();
        }
        private async ValueTask OffBettingTime()
        {
            _currentGameSession.BettingTime = false;
            _dbContext.Update<GameSession>(_currentGameSession);
            await _dbContext.SaveChangesAsync();
        }

        private async void StopBettingTimeCallback(object? state)
        {
            string crashJson = JsonConvert.SerializeObject(new { Message = "Betting time stoped" });

            Task saveTask = _hubContext.Clients.All.SendAsync("StopBettingTime", crashJson);
            ValueTask offTask = OffBettingTime();

            await Task.WhenAll(saveTask, offTask.AsTask());
            await StartRound();
        }


    }

    public class CrashGameLogic
    {
        private Random random = new Random();
        double BankBalance = 1000;
        public (double, double, double) GenerateOdds()
        {
            double adjustmentFactor = BankBalance > 1000 ? 0.95 : (BankBalance < 500 ? 1.05 : 1); // Adjust these values accordingly

            double x1 = 1 + random.NextDouble() * 5 * adjustmentFactor;
            double x2 = (2.0 / 3 * x1) + random.NextDouble() * 3 * adjustmentFactor;
            double x3 = (1.0 / 2 * x2) + random.NextDouble() * 2 * adjustmentFactor;

            return (x1, x2, x3);
        }
    }
}
