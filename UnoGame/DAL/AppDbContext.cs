using Microsoft.EntityFrameworkCore;
using UnoGame.DAL.Entity;

namespace UnoGame.DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<GameEntity> Games { get; set; }
        public DbSet<PlayerEntity> Players { get; set; }
        public DbSet<PlayerHandEntity> PlayerHands { get; set; }
        public DbSet<CardEntity> Cards { get; set; }
        public DbSet<PlayerHandSummaryEntity> PlayerHandSummaries { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // Example: Configure a one-to-many relationship between GameEntity and Player
            modelBuilder.Entity<GameEntity>()
                .HasMany(g => g.Players)
                .WithOne(p => p.Game)
                .HasForeignKey(p => p.GameId)
                .OnDelete(DeleteBehavior.Cascade); 
            
            modelBuilder.Entity<PlayerEntity>()
                .HasOne(p => p.Hand)  
                .WithOne(h => h.Player)
                .HasForeignKey<PlayerHandEntity>(h => h.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);
            

            // Add similar configurations for other relationships

            base.OnModelCreating(modelBuilder);
        }
    }
}