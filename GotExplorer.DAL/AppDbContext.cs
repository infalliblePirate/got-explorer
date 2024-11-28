using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GotExplorer.DAL.Entities;
using System.Data;
using System.Reflection.Emit;

namespace GotExplorer.DAL
{
    public class AppDbContext : IdentityDbContext<User,UserRole,int>
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
                .WithMany();

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

            builder.HasPostgresEnum<GameType>();

            builder.Entity<User>()
                .Property(e => e.ImageId)
                .HasDefaultValue(1);

            builder.Entity<Image>().HasData(
                new Image { Id = 1, Name = "", Path = "" }
            );
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Model3D> Models3D { get; set; }
        public DbSet<Image> Images { get; set; }    
    }
}
