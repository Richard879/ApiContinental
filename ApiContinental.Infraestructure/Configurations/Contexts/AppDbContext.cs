using Microsoft.EntityFrameworkCore;
using ApiContinental.Domain.Entities;

namespace ApiContinental.Infraestructure.Configurations.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<ImcRecord> ImcRecords { get; set; }
        public DbSet<ImcCategory> ImcCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ImcRecord>().HasKey(x => x.Id);
            modelBuilder.Entity<ImcCategory>().HasKey(x => x.Id);

            // Precision/scale explícitas para evitar truncamiento en SQL Server
            modelBuilder.Entity<ImcCategory>(entity =>
            {
                entity.Property(e => e.MinImc).HasPrecision(5, 2);
                entity.Property(e => e.MaxImc).HasPrecision(5, 2);
                entity.Property(e => e.Description).HasMaxLength(250);
            });

            modelBuilder.Entity<ImcRecord>(entity =>
            {
                entity.Property(e => e.ImcValue).HasPrecision(5, 2);
                entity.Property(e => e.WeightKg).HasPrecision(6, 2);
                entity.Property(e => e.Name).HasMaxLength(200);
                entity.Property(e => e.ImcDescription).HasMaxLength(250);
            });
        }

    }
}
