using CrashGameService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrashGameService.Context
{
    public class CrashDbContext : DbContext
    {
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<GameRound> GameRounds { get; set; }
        public DbSet<CashOut> CashOuts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameSession>()
                .HasOne(s => s.CurrentRound)
                .WithOne()
                .HasForeignKey<GameSession>(s => s.CurrentRoundId);

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
                .WithMany(co => co.CashOuts)
                .HasForeignKey(co => co.BetId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //crash-game-db
            optionsBuilder.UseNpgsql("Host=localhost:5432;Username=postgres;Password=aqw1!AsdfKp25735lsd112312fASsadsqwewesadsa122Q354584123@AWECcerw2;Database=CrashGame;");
        }
    }
}
