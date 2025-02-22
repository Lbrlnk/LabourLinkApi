using JobPostService.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPostService.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		public DbSet<JobPost> JobPost { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<JobPost>()
				.Property(x => x.JobId)
				.HasDefaultValueSql("NEWSEQUENTIALID()");

			modelBuilder.Entity<JobPost>()
			.Property(m => m.CreatedDate)
			.HasDefaultValueSql("GETUTCDATE()");
			modelBuilder.Entity<JobPost>()
				.Property(p => p.Wage)
				.HasColumnType("decimal(10,2)");

			base.OnModelCreating(modelBuilder);
		}
	}
	
}
