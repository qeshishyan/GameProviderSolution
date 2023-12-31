﻿using CrashGameService.DAL.IRepository;
using CrashGameService.Repository.Entities;
using CrashGameService.Service.HttpClients;
using CrashGameService.Service.Models;
using Shared.Exceptions;

namespace CrashGameService.Service.Services
{
    public class GameService : IGameService
    {
        private readonly GameSession _currentGameSession;
        private readonly IGameRepository _repository;
        private readonly IGameLogicPhytonClient _client;
        private readonly IGameHubService _hubService;

        private double brokenJet;

        public GameService(IGameRepository repository,
            IGameLogicPhytonClient client, IGameHubService hubService)
        {
            _currentGameSession = new GameSession
            {
                ProviderId = "providerId_4578d687ad65",
            };
            _repository = repository;
            _client = client;
            _hubService = hubService;
        }

        public async ValueTask<GameSessionResponse> StartGame()
        {
            if (_currentGameSession.Started)
                throw new ApiException(400, "Game already started");

            _currentGameSession.Started = true;
            _currentGameSession.StartedDate = DateTime.UtcNow;

            int id = await _repository.AddSessionAsync(_currentGameSession);

            await StartBettingTime();
            return new GameSessionResponse
            {
                SessionId = id
            };
        }



        private async ValueTask StartRound()
        {
            _currentGameSession.CurrentRound.Started = true;
            _currentGameSession.CurrentRound.StartDate = DateTime.UtcNow;
            var round = _currentGameSession.CurrentRound;

            var update = _repository.UpdateRoundAsync(round);

            var obj = new { Message = "Round started" };
            Task sendTask = _hubService.SendToHub("StartRound", obj);
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
                await _hubService.SendToHub("ReceiveMultiplier", obj);
                Thread.Sleep(20);
            }
            await CrashGame(round);
        }



        private async ValueTask CrashGame(GameRound round)
        {
            round.IsCrashed = true;
            round.EndDate = DateTime.UtcNow;

            var obj = new { Message = "Game crashed", Multiplier = round.Multiplier.ToString("F2") };
            Task sendTask = _hubService.SendToHub("CrashGame", obj);

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
            Task sendTask = _hubService.SendToHub("StartBettingTime", obj);

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
            Task saveTask = _hubService.SendToHub("StopBettingTime", obj);
            ValueTask offTask = OffBettingTime();

            await Task.WhenAll(saveTask, offTask.AsTask());
            await StartRound();
        }

        public async ValueTask<MultiplierListResponse> GetLastMultipliers(int count, int sessionId)
        {
            var result = await _repository.GetLastMultipliersAsync(count, sessionId);
            return new MultiplierListResponse
            {
                Multipliers = result,
                SessionId = sessionId
            };
        }

        public async ValueTask<BetListResponse> GetLastBets(int count, int sessionId)
        {
            var result = await _repository.GetLastBetsAsync(count, sessionId, 50);
            List<UserBetResponse> list = result.Select(x => new UserBetResponse
            {
                UserName = "User_" + new Random().Next(1, 99).ToString(),
                BetValue = x.Value

            }).ToList();

            return new BetListResponse
            {
                Bets = list,
                SessionId = sessionId
            };
        }
    }
}
