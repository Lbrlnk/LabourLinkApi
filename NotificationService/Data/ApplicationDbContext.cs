using Microsoft.EntityFrameworkCore;
using NotificationService.Models;

namespace NotificationService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

       public  DbSet<InterestRequest> InterestRequests { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InterestRequest>(entity =>
            {

                entity.HasKey(e => e.Id);
                entity.Property(e => e.JobPostId).IsRequired();
                entity.Property(e => e.EmployerUserId).IsRequired();
                entity.Property(e => e.LabourUserId).IsRequired();
                entity.Property(e => e.Status).HasConversion<string>();

            });


            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NotificationType).HasConversion<string>();
                //entity.Property(e => e.Status).HasConversion<string>();
            });





                
                
        }

    }
}
