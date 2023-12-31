﻿using CrashGameService.DAL.IRepository;
using CrashGameService.Repository.Context;
using CrashGameService.Repository.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrashGameService.DAL.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly CrashDbContext _dbContext;
        public GameRepository(CrashDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddSessionAsync(GameSession gameSession)
        {
            await _dbContext.GameSessions.AddAsync(gameSession);
            await _dbContext.SaveChangesAsync();
            return gameSession.Id;
        }

        public async Task<GameRound> GetRoundWithSessionAsync(int roundId)
        {
            try
            {
                return await _dbContext.GameRounds
                    .Include(x=> x.GameSession)
                    .Where(x => x.Id == roundId)
                    .FirstAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Bet> GetBetWithRoundAsync(int betId)
        {
            try
            {
                return await _dbContext.Bets
                    .Include(x => x.GameRound)
                    .Where(x => x.Id == betId)
                    .FirstAsync();
            }
            catch (Exception)
            {

                throw;
            }
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

        public async Task<List<double>> GetLastMultipliersAsync(int count, int sessionId)
        {
            try
            {
                var result = await _dbContext.GameRounds
                    .Where(x=> x.GameSessionId == sessionId)
                    .OrderByDescending(x => x.Id)
                    .Take(count)
                    .Select(x => x.Multiplier)
                    .ToListAsync();

                return result;
                    
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<Bet>> GetLastBetsAsync(int count, int sessionId, int maxCount)
        {
            try
            {
                if (count > maxCount)
                    count = maxCount;

                var result = await _dbContext.Bets
                    .Include(x=> x.GameRound)
                    .Where(x => x.GameRound.GameSessionId == sessionId)
                    .OrderByDescending(x => x.Id)
                    .Take(count)
                    .ToListAsync();

                return result;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
