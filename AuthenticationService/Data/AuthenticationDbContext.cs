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

        public DbSet<Labour> Labours { get; set; }
        public DbSet<Employer> Employers { get; set; }

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



            modelBuilder.Entity<Labour>()
                .HasOne(l => l.User)
                .WithMany(u => u.Labours) // Assuming User doesn't have a collection of Labours
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Employer>()
        .HasOne(e => e.User)
        .WithMany(u => u.Employers) // Assuming User doesn't have a collection of Employers
        .HasForeignKey(e => e.UserId)
        .OnDelete(DeleteBehavior.Cascade);

        }



    }
}
