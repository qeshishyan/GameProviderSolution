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
            StartBettingTime();
            await Task.CompletedTask;
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
            object obj = new { Message = "BET", Type = 2001, Value = bet.Value, User = "User_" + Guid.NewGuid().ToString() };
            var betJson = JsonConvert.SerializeObject(obj);
            Task task = _hubContext.Clients.All.SendAsync("BET", betJson);

            // Save bet in db
            var response = new BetResponse { BetDate = bet.BetDate, GameRoundId = bet.GameRoundId, BetId = bet.Id };
            task.Wait();
            return await Task.FromResult(response);
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

        public async Task<double> CashOut(int betId, double multiplier)
        {
            //Save cash out in db

            return await Task.FromResult(multiplier);
        }


        private void StartRound()
        {
            var random = new Random();
            var round = _currentGameSession.CurrentRound;

            //Send socket for round started
            var json = JsonConvert.SerializeObject(new { Message = "Round started", Type = 2001 });
            Task task = _hubContext.Clients.All.SendAsync("StartRound", json);
            //round Save in db 
            task.Wait();

            while (!_token.IsCancellationRequested)
            {
                round.Multiplier += 0.1d;
                var multipJson = JsonConvert.SerializeObject(new { Message = "Multiplier value", Type = 3001, Multiplier = round.Multiplier.ToString("F2") });
                task = _hubContext.Clients.All.SendAsync("ReceiveMultiplier", multipJson);
                task.Wait();

                //Change to real game logic
                if (random.Next(3000, 9000) > 8900)
                {
                    var crashJson = JsonConvert.SerializeObject(new { Message = "Game crashed", Type = 2002, Multiplier = round.Multiplier.ToString("F2") });
                    task = _hubContext.Clients.All.SendAsync("CrashGame", crashJson);

                    round.IsCrashed = true;
                    round.EndDate = DateTime.Now;

                    task.Wait();
                    Thread.Sleep(2000);
                    break;
                }
                Thread.Sleep(100);
            }
            StartBettingTime();
        }

        private GameRound CreateRound() => 
            _currentGameSession.CurrentRound = new()
            {
                // Database key
                Id = new Random().Next(10000, 99999),
                GameSessionId = _currentGameSession.Id,
                CreatedDate = DateTime.Now
            };

        private void StartBettingTime()
        {
            CreateRound();
            Timer _timer = new(StopBettingTimeCallback, null, 5000, Timeout.Infinite);
            _currentGameSession.BettingTime = true;

            var crashJson = JsonConvert.SerializeObject(new { Message = "Betting time started", Type = 1001, CurrentRoundId = _currentGameSession.CurrentRound.Id });
            Task task = _hubContext.Clients.All.SendAsync("StartBettingTime", crashJson);
            task.Wait();
        }

        private void StopBettingTimeCallback(object? state)
        {
            var crashJson = JsonConvert.SerializeObject(new { Message = "Betting time stoped", Type = 1002 });
            Task task = _hubContext.Clients.All.SendAsync("StopBettingTime", crashJson);

            _currentGameSession.BettingTime = false;
            _currentGameSession.CurrentRound.Started = true;
            _currentGameSession.CurrentRound.StartDate = DateTime.Now;
            
            StartRound();
            task.Wait();
        }
    }
}
