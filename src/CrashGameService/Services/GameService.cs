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
            //Save session in db
        }

        public async Task StartGame()
        {
            StartBettingTime();
            await Task.CompletedTask;
        }

        public async Task<BetResponse> Bet(Bet bet)
        {
            if (_currentGameSession.CurrentRound == null)
                throw new ApiException(400, "Game not started");

            if (!_currentGameSession.BettingTime)
                throw new ApiException(400, "Betting time is closed");

            if (bet.Value <= 0)
                throw new ApiException(400, "Invalid bet value");

            if (bet.Multiplier > _currentGameSession.CurrentRound.Multiplier)
                throw new ApiException(400, "Invalid bet multiplier");

            bet.BetDate = DateTime.Now;
            bet.GameRoundId = _currentGameSession.CurrentRound.Id;
            bet.Id = 1540;

            // Save bet in db
            var response = new BetResponse { BetDate = bet.BetDate, GameRoundId = bet.GameRoundId, BetId = bet.Id };
            var task = Task.FromResult(response);
            return await task;
        }

        public async Task<double> CashOut(int betId, double multiplier)
        {
            //Save cash out in db

            return await Task.FromResult(multiplier);
        }


        private void StartRound()
        {
            //Send socket for round started
            var json = JsonConvert.SerializeObject(new { Message = "Round started", Type = 2001 });
            Task task = _hubContext.Clients.All.SendAsync("StartRound", json);

            var random = new Random();
            GameRound round = new()
            {
                GameSessionId = _currentGameSession.Id,
                StartDate = DateTime.Now
            };
            _currentGameSession.CurrentRound = round;
            //round Save in db 
            task.Wait();

            while (!_token.IsCancellationRequested)
            {
                round.Multiplier += 0.1d;
                var multipJson = JsonConvert.SerializeObject(new { Message = "Multiplier value", Type = 3001, Multiplier = round.Multiplier });
                task = _hubContext.Clients.All.SendAsync("ReceiveMultiplier", multipJson);
                task.Wait();

                if(random.Next(3000, 9000) > 8800)
                {
                    var crashJson = JsonConvert.SerializeObject(new { Message = "Game crashed", Type = 2002, Multiplier = round.Multiplier });
                    task = _hubContext.Clients.All.SendAsync("CrashGame", crashJson);

                    round.IsCrashed = true;
                    round.EndDate = DateTime.Now;

                    task.Wait();
                    Thread.Sleep(2000);
                    break;
                }
                Thread.Sleep(200);
            }
            StartBettingTime();
        }

        private void StartBettingTime()
        {
            Timer _timer = new(StopBettingTimeCallback, null, 5000, Timeout.Infinite);
            _currentGameSession.BettingTime = true;

            var crashJson = JsonConvert.SerializeObject(new { Message = "Betting time started", Type = 1001 });
            Task task = _hubContext.Clients.All.SendAsync("StartBettingTime", crashJson);
            task.Wait();
        }

        private void StopBettingTimeCallback(object? state)
        {
            var crashJson = JsonConvert.SerializeObject(new { Message = "Betting time stoped", Type = 1002 });
            Task task = _hubContext.Clients.All.SendAsync("StopBettingTime", crashJson);

            _currentGameSession.BettingTime = false;
            StartRound();
            task.Wait();
        }
    }
}
