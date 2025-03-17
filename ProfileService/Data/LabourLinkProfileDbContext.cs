using Microsoft.EntityFrameworkCore;
using ProfileService.Models;

namespace ProfileService.Data
{
    public class LabourLinkProfileDbContext : DbContext
    {
        public LabourLinkProfileDbContext(DbContextOptions<LabourLinkProfileDbContext> options) : base(options) { }

        public DbSet<Labour> Labours {get; set;}
        public DbSet<Employer> Employers {get; set;}

        public DbSet<LabourPreferredMuncipality> LabourPreferedMuncipalities {get; set;}
        public DbSet<LabourWorkImage> LabourWorkImages {get; set;}
        public DbSet<LabourSkills> LabourSkills {get; set;}
        public DbSet<Review> Reviews { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Labour>(entity =>
            {
                entity.HasKey(l => l.LabourId);
                entity.HasIndex(l => l.PhoneNumber).IsUnique();
                entity.Property(l => l.FullName).IsRequired().HasMaxLength(100);
                entity.Property(l => l.Rating).HasDefaultValue(0);
                entity.Property(l => l.PreferedTime).HasConversion<string>();
                entity.Property(l => l.IsActive).HasDefaultValue(true);
                entity.HasMany(l => l.LabourWorkImages)
                      .WithOne(lw => lw.Labour)
                      .OnDelete(DeleteBehavior.Cascade);
         
                     
                entity.HasMany(l => l.LabourSkills)
                      .WithOne(ls => ls.Labour)
                      .OnDelete(DeleteBehavior.Cascade);

                
                
            });

            modelBuilder.Entity<Employer>(entity =>
            {
                entity.HasKey(e => e.EmployerId);
                entity.HasIndex(e => e.PhoneNumber).IsUnique();
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<LabourSkills>(entity =>
            {
                entity.HasKey(ls => ls.Id);
                entity.HasOne(ls => ls.Labour)
                      .WithMany(l => l.LabourSkills)
                      .OnDelete(DeleteBehavior.Cascade);    
                     
            });
            modelBuilder.Entity<LabourPreferredMuncipality>(entity =>
            {


                entity.HasKey(lpm => lpm.Id);
                entity.HasOne(l => l.Labour)
                      .WithMany(l => l.LabourPreferedMuncipalities)
                      .HasForeignKey(l => l.LabourId) 
                      .OnDelete(DeleteBehavior.Cascade);

            }
            );
            modelBuilder.Entity<LabourWorkImage>(entity =>
            {
                entity.HasKey(lw => lw.Id);
                entity.HasOne(lw => lw.Labour)
                      .WithMany(l => l.LabourWorkImages)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Review>(entity =>
			{
				entity.Property(r => r.Rating)
		              .HasColumnType("decimal(3,2)")  
		              .IsRequired();

				entity.HasOne(r => r.Employer)
		              .WithMany(c => c.Reviews)
		              .HasForeignKey(r => r.EmployerId)
		              .OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(r => r.Labour)
					  .WithMany(l => l.Reviews)
					  .HasForeignKey(r => r.LabourId)
					  .OnDelete(DeleteBehavior.Cascade);

			});
			//modelBuilder.Entity<JobPost>()
		 //   .Property(j => j.Wage)
		 //   .HasColumnType("decimal(18,2)");
		}

    }
}
