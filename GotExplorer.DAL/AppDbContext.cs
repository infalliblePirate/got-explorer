using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GotExplorer.DAL.Entities;
using System.Data;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GotExplorer.DAL
{
    public class AppDbContext : IdentityDbContext<User, UserRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
                .HasOne(e => e.Image)
                .WithMany()
                .HasForeignKey(e => e.ImageId)
                .IsRequired();

            builder.Entity<Level>()
                .HasMany(e => e.Models)
                .WithMany();

            builder.Entity<Game>()
                .HasMany(e => e.Levels)
                .WithMany()
                .UsingEntity<GameLevel>();

            builder.Entity<Game>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            builder.Entity<Image>()
                .Property(e => e.Version)
                .IsRowVersion();

            builder.Entity<Model3D>()
                .Property(e => e.Version)
                .IsRowVersion();

            builder.Entity<Level>()
                .Property(e => e.Version)
                .IsRowVersion();

            builder.Entity<GameLevel>()
                .HasKey(e => new { e.GameId, e.LevelId });

            builder.Entity<GameLevel>()
                .HasOne(e => e.Level)
                .WithMany()
                .HasForeignKey(e => e.LevelId)
                .IsRequired();

            builder.Entity<GameLevel>()
                .HasOne(e => e.Game)
                .WithMany()
                .HasForeignKey(e => e.GameId)
                .IsRequired();

            builder.HasPostgresEnum<GameType>();

            Guid defaultImageId = Guid.NewGuid();
            builder.Entity<User>()
                .Property(e => e.ImageId)
                .HasDefaultValue(defaultImageId);

            builder.Entity<Image>().HasData(
                new Image { Id = defaultImageId, Name = "", Path = "" }
            );
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Model3D> Models3D { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<GameLevel> GameLevels { get; set; }
    }
}
