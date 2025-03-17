using Microsoft.EntityFrameworkCore;
using NotificationService.Models;

namespace NotificationService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        DbSet<InterestRequest> InterestRequests { get; set; }
        DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InterestRequest>(entity =>
            {

                entity.HasKey(e => e.Id);
                entity.Property(e => e.JobPostId).IsRequired();
                entity.Property(e => e.EmployerUserId).IsRequired();
                entity.Property(e => e.LabourerUserId).IsRequired();
                entity.Property(e => e.Status).HasConversion<string>();

            });


            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.notificationType).HasConversion<string>();
                entity.Property(e => e.Status).HasConversion<string>();
            });





                
                
        }

    }
}
