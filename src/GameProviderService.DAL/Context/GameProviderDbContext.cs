using GameProvider.Repository.Entities;
using GameProviderService.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GameProviderService.Repository.Context
{
    public class GameProviderDbContext : DbContext
    {
        private readonly ContextOptions _options;
        public GameProviderDbContext(IOptions<ContextOptions> options)
        {
            _options = options.Value;
        }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Lobby> Lobbies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the Lobby entity
            modelBuilder.Entity<Lobby>(entity =>
            {
                entity.HasKey(l => l.Id); // Primary key
                entity.Property(l => l.Key).HasMaxLength(255); // Optionally set a max length
                entity.Property(l => l.GameUrl).HasMaxLength(255); // Optionally set a max length

                // If you want to configure any default values, indexes, etc., you can do it here
            });

            // Configuring the Merchant entity
            modelBuilder.Entity<Merchant>(entity =>
            {
                entity.HasKey(m => m.Id); // Primary key
                entity.Property(m => m.MerchantId).HasMaxLength(255); // Optionally set a max length
                entity.Property(m => m.Name).HasMaxLength(255); // Optionally set a max length
                entity.Property(m => m.Address).HasMaxLength(500); // Optionally set a max length
                entity.Property(m => m.Url).HasMaxLength(500); // Optionally set a max length

                // Configure the foreign key relationship with Lobby
                entity.HasOne(m => m.Lobby)
                      .WithMany(l => l.Merchants)
                      .HasForeignKey(m => m.LobbyId)
                      .OnDelete(DeleteBehavior.Restrict); // Or use another delete behavior as needed
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //game-provider-db
            optionsBuilder.UseNpgsql(_options.ConnectionString);
        }
    }
}
