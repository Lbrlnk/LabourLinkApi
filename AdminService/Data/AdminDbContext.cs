using AdminService.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Data
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options) { }

        public DbSet<Muncipality> Muncipalities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Muncipality>()
                .HasKey(m => m.MunicipalityId);

			modelBuilder.Entity<Muncipality>()
		    .Property(m => m.CreatedAt)
		    .HasDefaultValueSql("GETUTCDATE()");

			modelBuilder.Entity<Muncipality>()
	        .Property(m => m.IsActive)
	        .HasDefaultValue(true);

			modelBuilder.Entity<Muncipality>().HasData(
				new Muncipality { MunicipalityId = 1, Name = "Thirurangadi",State="Kerala",IsActive=true,CreatedAt= new DateTime(2025, 2, 4) },
				new Muncipality { MunicipalityId = 2, Name = "Kondotty", State = "Kerala", IsActive = true, CreatedAt = new DateTime(2025, 2, 4) }
				);
			base.OnModelCreating(modelBuilder);
        }
    }
}
