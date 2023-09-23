using CrashGameService.DAL.Context;
using CrashGameService.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CrashGameService.Repository.Context
{
    public class CrashDbContext : DbContext
    {
        private readonly ContextOptions _options;
        public CrashDbContext(IOptions<ContextOptions> options)
        {
            _options = options.Value;
        }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<GameRound> GameRounds { get; set; }
        public DbSet<CashOut> CashOuts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameSession>()
                .HasOne(s => s.CurrentRound)
                .WithOne()
                .HasForeignKey<GameSession>(s => s.CurrentRoundId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GameRound>()
                .HasOne(gr => gr.GameSession)
                .WithMany(gr => gr.GameRounds)
                .HasForeignKey(gr => gr.GameSessionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bet>()
                .HasOne(b => b.GameRound)
                .WithMany(b => b.Bets)
                .HasForeignKey(b => b.GameRoundId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CashOut>()
                .HasOne(co => co.Bet)
                .WithOne()
                .HasForeignKey<CashOut>(s => s.BetId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //crash-game-db
            optionsBuilder.UseNpgsql(_options.ConnectionString);
        }
    }
}
