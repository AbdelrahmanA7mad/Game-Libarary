using GameApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GameApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<GameDevice> GameDevices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.GameDevices)
                .WithMany(d => d.Games)
                .UsingEntity(j => j.ToTable("GameDeviceGames"));

            modelBuilder.Entity<Game>()
                .HasOne(g => g.category)
                .WithMany(c => c.Games)
                .HasForeignKey(g => g.Categoryid)
                .OnDelete(DeleteBehavior.Cascade);

            // Adding default categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action" },
                new Category { Id = 2, Name = "Adventure" },
                new Category { Id = 3, Name = "Puzzle" },
                new Category { Id = 4, Name = "RPG" }
            );

            // Adding default game devices
            modelBuilder.Entity<GameDevice>().HasData(
                new GameDevice { Id = 1, Name = "PC" },
                new GameDevice { Id = 2, Name = "PlayStation" },
                new GameDevice { Id = 3, Name = "Xbox" },
                new GameDevice { Id = 4, Name = "Mobile" }
            );
        }
    }
}
