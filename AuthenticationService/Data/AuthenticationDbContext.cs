using AuthenticationService.Enums;
using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Data
{
    public class AuthenticationDbContext : DbContext
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options ) : base(options) { }  

        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
              .WithMany(u => u.RefreshTokens)
              .HasForeignKey(rt => rt.UserId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .Property(u=>u.UserType)
                 .HasConversion<string>();




        }



    }
}
