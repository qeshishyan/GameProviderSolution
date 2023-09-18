﻿using CrashGameService.DAL.IRepository;
using CrashGameService.Repository.Context;
using CrashGameService.Repository.Entities;

namespace CrashGameService.DAL.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly CrashDbContext _dbContext;
        public GameRepository(CrashDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddSessionAsync(GameSession gameSession)
        {
            await _dbContext.GameSessions.AddAsync(gameSession);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddBetAsync(Bet bet)
        {
            await _dbContext.Bets.AddAsync(bet);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRoundAsync(GameRound round)
        {
            _dbContext.Update<GameRound>(round);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRoundAsync(GameRound round)
        {
            await _dbContext.GameRounds.AddAsync(round);
            await _dbContext.SaveChangesAsync();
        }
        public async Task AddCashOutAsync(CashOut cashOut)
        {
            await _dbContext.CashOuts.AddAsync(cashOut);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateGameSessionAsync(GameSession session)
        {
            _dbContext.Update<GameSession>(session);
            await _dbContext.SaveChangesAsync();
        }
    }
}
