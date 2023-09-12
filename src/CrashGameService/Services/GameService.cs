using CrashGameService.Entities;
using Shared.Exceptions;

namespace CrashGameService.Services
{
    public class GameService : IGameService
    {
        private readonly GameSession _currentGameSession;
        private readonly CancellationToken _token;
        public GameService(string providerId, CancellationTokenSource tokenSource)
        {
            _currentGameSession = new GameSession
            {
                ProviderId = providerId,
            };
            _token = tokenSource.Token;
            //Save session in db
        }

        public async Task StartGame()
        {
            StartBettingTime();
            await Task.CompletedTask;
        }

        public async Task<Bet> Bet(Bet bet)
        {
            if(_currentGameSession.CurrentRound == null)
                throw new ApiException(400, "Game not started");

            if (!_currentGameSession.BettingTime)
                throw new ApiException(400, "Betting time is closed");

            if (bet.Value <= 0)
                throw new ApiException(400, "Invalid bet value");

            if (bet.Multiplier > _currentGameSession.CurrentRound.Multiplier)
                throw new ApiException(400, "Invalid bet multiplier");

            bet.BetDate = DateTime.Now;
            // Save bet in db
            bet.Id = 1540;
            return await Task.FromResult(bet);
        }

        public async Task<double> CashOut(int betId, double multiplier)
        {
            //Save cash out in db

            return await Task.FromResult(multiplier);
        }

        private void StartRound()
        {
            //Send socket for round started

            var random = new Random();
            GameRound round = new()
            {
                GameSessionId = _currentGameSession.Id,
                StartDate = DateTime.Now
            };
            _currentGameSession.CurrentRound = round;
            //round Save in db 

            while (!_token.IsCancellationRequested)
            {
                round.Multiplier += 0.01;

                if (random.Next(10, 100) < 20)
                {
                    round.IsCrashed = true;
                    round.EndDate = DateTime.Now;

                    //Send socket for crashed

                    break;
                }
                Thread.Sleep(500);
            }
            StartBettingTime();
        }

        private void StartBettingTime()
        {
            //Callback to front by socket for starting betting time

            Timer _timer = new(StopBettingTimeCallback, null, 5000, Timeout.Infinite);
            _currentGameSession.BettingTime = true;
        }

        private void StopBettingTimeCallback(object? state)
        {
            _currentGameSession.BettingTime = false;
            StartRound();
        }
    }
}
