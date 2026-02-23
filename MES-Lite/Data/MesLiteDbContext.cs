using MES_Lite.MesEntities;
using Microsoft.EntityFrameworkCore;

namespace MES_Lite.Data
{
    public class MesLiteDbContext : DbContext
    {
        public MesLiteDbContext(DbContextOptions<MesLiteDbContext> options)
            : base(options)
        {
        }

        public DbSet<MaterialDefinition> MaterialDefinitions => Set<MaterialDefinition>();
        public DbSet<MaterialLot> MaterialLots => Set<MaterialLot>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // MaterialDefinition
            modelBuilder.Entity<MaterialDefinition>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<MaterialDefinition>()
                .HasMany(m => m.Lots)
                .WithOne(l => l.MaterialDefinition)
                .HasForeignKey(l => l.MaterialDefinitionId);

            // MaterialLot
            modelBuilder.Entity<MaterialLot>()
                .HasKey(l => l.Id);

            modelBuilder.Entity<MaterialLot>()
                .HasIndex(l => l.LotId)
                .IsUnique();

            modelBuilder.Entity<MaterialLot>()
                .Property(m => m.Quantity)
                .HasPrecision(18, 4);
        }
    }
}
