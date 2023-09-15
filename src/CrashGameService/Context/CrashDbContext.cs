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
            // Relationship between GameSession and its GameRounds
            modelBuilder.Entity<GameRound>()
                .HasOne(gr => gr.Session)
                .WithMany(gs => gs.GameRounds)  
                .HasForeignKey(gr => gr.GameSessionId)  
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Bet>()
                .HasOne(gr => gr.Round)
                .WithMany(gs => gs.Bets)   
                .HasForeignKey(gr => gr.GameRoundId) 
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<CashOut>()
                .HasOne(gr => gr.Bet)
                .WithMany(gs => gs.CashOuts) 
                .HasForeignKey(gr => gr.BetId) 
                .OnDelete(DeleteBehavior.Restrict); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=crash-game-db:5432;Username=postgres;Password=554466;Database=CrashGame;");
        }
    }
}
