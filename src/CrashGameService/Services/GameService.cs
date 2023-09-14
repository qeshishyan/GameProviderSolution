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
        public GameService(IHubContext<GameHub> hubContext)
        {
            _currentGameSession = new GameSession
            {
                ProviderId = "providerId_4578d687ad65",
            };
            _token = new CancellationTokenSource().Token;
            _hubContext = hubContext;
        }

        public async Task StartGame()
        {
            //Save session in db

            // Database key
            _currentGameSession.Id = new Random().Next(10000, 99999);
            _currentGameSession.Started = true;
            _currentGameSession.StartedDate = DateTime.Now;
            await StartBettingTime();
        }

        public async Task<BetResponse> Bet(Bet bet)
        {
            ValidateBet(bet);

            bet.BetDate = DateTime.Now;
            bet.GameRoundId = _currentGameSession.CurrentRound.Id;
            // Database key
            bet.Id = new Random().Next(10000, 99999);
            _currentGameSession.CurrentRound.Bets.Add(bet);

            //User id must be changed to real data
            object obj = new { Message = "BET", Value = bet.Value, User = "User_" + Guid.NewGuid().ToString() };
            var betJson = JsonConvert.SerializeObject(obj);
            await _hubContext.Clients.All.SendAsync("BET", betJson);

            // Save bet in db
            var response = new BetResponse { BetDate = bet.BetDate, GameRoundId = bet.GameRoundId, BetId = bet.Id };
            return response;
        }

        private void ValidateBet(Bet bet)
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

        public async Task<CashOutResponse> CashOut(CashOut cashOut)
        {
            //Save cash out in db

            var bet = _currentGameSession.CurrentRound.Bets.Where(x => x.Id == cashOut.BetId).FirstOrDefault();
            if (bet is null)
                throw new ApiException(400, "Bet not found");

            ValidateCashOut(cashOut, bet);

            bet.Win = true;

            return await Task.FromResult(new CashOutResponse { Value = bet.Value * cashOut.Multiplier });
        }

        private void ValidateCashOut(CashOut cashOut, Bet? bet)
        {
            if (cashOut.Multiplier > _currentGameSession.CurrentRound.Multiplier)
                throw new ApiException(400, "Invalid multiplier");
        }

        private async Task StartRound()
        {
            var random = new Random();
            var round = _currentGameSession.CurrentRound;

            //Send socket for round started
            var json = JsonConvert.SerializeObject(new { Message = "Round started" });
            await _hubContext.Clients.All.SendAsync("StartRound", json);
            //round Save in db 

            while (!_token.IsCancellationRequested)
            {
                round.Multiplier += 0.1d;
                var multipJson = JsonConvert.SerializeObject(new { Message = "Multiplier", Multiplier = round.Multiplier.ToString("F2") });
                await _hubContext.Clients.All.SendAsync("ReceiveMultiplier", multipJson);

                //Change to real game logic
                if (random.Next(3000, 9000) > 8900)
                {
                    var crashJson = JsonConvert.SerializeObject(new { Message = "Game crashed", Multiplier = round.Multiplier.ToString("F2") });
                    round.IsCrashed = true;
                    round.EndDate = DateTime.Now;
                    await _hubContext.Clients.All.SendAsync("CrashGame", crashJson);

                    Thread.Sleep(2000);
                    break;
                }
                Thread.Sleep(100);
            }
            await StartBettingTime();
        }

        private GameRound CreateRound() => 
            _currentGameSession.CurrentRound = new()
            {
                // Database key
                Id = new Random().Next(10000, 99999),
                GameSessionId = _currentGameSession.Id,
                CreatedDate = DateTime.Now
            };

        private async Task StartBettingTime()
        {
            CreateRound();
            Timer _timer = new(StopBettingTimeCallback, null, 10000, Timeout.Infinite);
            _currentGameSession.BettingTime = true;

            var crashJson = JsonConvert.SerializeObject(new { Message = "Betting time started", CurrentRoundId = _currentGameSession.CurrentRound.Id });
            await _hubContext.Clients.All.SendAsync("StartBettingTime", crashJson);
        }

        private void StopBettingTimeCallback(object? state)
        {
            var crashJson = JsonConvert.SerializeObject(new { Message = "Betting time stoped" });
            Task task = _hubContext.Clients.All.SendAsync("StopBettingTime", crashJson);

            _currentGameSession.BettingTime = false;
            _currentGameSession.CurrentRound.Started = true;
            _currentGameSession.CurrentRound.StartDate = DateTime.Now;
            
            Task startTask = StartRound();
            Task.WaitAll(startTask, task);
        }
    }
}
