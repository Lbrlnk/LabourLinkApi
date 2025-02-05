using AdminService.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Data
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options) : base(options) { }

        public DbSet<Muncipality> Muncipalities { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Muncipality>()
                .HasKey(m => m.MunicipalityId);

            modelBuilder.Entity<Skill>()
                .Property(x => x.SkillId)
                .HasDefaultValueSql("NEWSEQUENTIALID()");


            base.OnModelCreating(modelBuilder);
        }
    }
}
