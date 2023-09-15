﻿using AutoMapper;
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

        public async Task StartGame()
        {
            _currentGameSession.Started = true;
            _currentGameSession.StartedDate = DateTime.UtcNow;

            await _dbContext.GameSessions.AddAsync(_currentGameSession);
            await _dbContext.SaveChangesAsync();

            await StartBettingTime();
        }

        public async Task<BetResponse> Bet(BetRequest betRequest)
        {
            ValidateBet(betRequest);

            var bet = _mapper.Map<Bet>(betRequest);
            bet.BetDate = DateTime.UtcNow;
            bet.Round = _currentGameSession.CurrentRound;

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

        public async Task<CashOutResponse> CashOut(CashOutRequest cashOutRequest)
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

        private async Task StartRound()
        {
            var random = new Random();


            _currentGameSession.CurrentRound.Started = true;
            _currentGameSession.CurrentRound.StartDate = DateTime.UtcNow;
            var round = _currentGameSession.CurrentRound;

            _dbContext.Update<GameRound>(round);
            var saveTask = _dbContext.SaveChangesAsync();

            //Send socket for round started
            var json = JsonConvert.SerializeObject(new { Message = "Round started" });
            Task sendTask = _hubContext.Clients.All.SendAsync("StartRound", json);

            await Task.WhenAll(saveTask, sendTask);

            while (!_token.IsCancellationRequested)
            {
                round.Multiplier += 0.1d;
                var multipJson = JsonConvert.SerializeObject(new { Message = "Multiplier", Multiplier = round.Multiplier.ToString("F2") });
                await _hubContext.Clients.All.SendAsync("ReceiveMultiplier", multipJson);

                //Change to real game logic
                if (random.Next(3000, 9000) > 8900)
                {
                    round.IsCrashed = true;
                    round.EndDate = DateTime.UtcNow;
                    
                    var crashJson = JsonConvert.SerializeObject(new { Message = "Game crashed", Multiplier = round.Multiplier.ToString("F2") });
                    sendTask = _hubContext.Clients.All.SendAsync("CrashGame", crashJson);

                    _dbContext.Update<GameRound>(round);
                    saveTask = _dbContext.SaveChangesAsync();

                    await Task.WhenAll(saveTask, sendTask);

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
                //Id = new Random().Next(10000, 99999),
                GameSessionId = _currentGameSession.Id,
                CreatedDate = DateTime.UtcNow
            };

        private async Task StartBettingTime()
        {
            CreateRound();
            Timer _timer = new(StopBettingTimeCallback, null, 10000, Timeout.Infinite);

            var crashJson = JsonConvert.SerializeObject(new { Message = "Betting time started", CurrentRoundId = _currentGameSession.CurrentRound.Id });
            Task sendTask = _hubContext.Clients.All.SendAsync("StartBettingTime", crashJson);
            Task betTimeTask = OnBettingTime();
            await Task.WhenAll(sendTask, betTimeTask);
        }

        private async Task OnBettingTime()
        {
            _currentGameSession.BettingTime = true;
            _dbContext.Update<GameSession>(_currentGameSession);
            await _dbContext.SaveChangesAsync();
        }
        private async Task OffBettingTime()
        {
            _currentGameSession.BettingTime = false;
            _dbContext.Update<GameSession>(_currentGameSession);
            await _dbContext.SaveChangesAsync();
        }

        private async void StopBettingTimeCallback(object? state)
        {
            var crashJson = JsonConvert.SerializeObject(new { Message = "Betting time stoped" });
            
            var saveTask = _hubContext.Clients.All.SendAsync("StopBettingTime", crashJson);
            var offTask = OffBettingTime();

            await Task.WhenAll(saveTask, offTask);
            await StartRound();
        }

        
    }
}
