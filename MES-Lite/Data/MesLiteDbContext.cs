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
        public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
        public DbSet<MaterialRequirement> MaterialRequirements => Set<MaterialRequirement>();
        public DbSet<Equipment> Equipment => Set<Equipment>();
        public DbSet<Personnel> Personnel => Set<Personnel>();



        // Configurazione del modello e delle relazioni
        // Qui definiamo le chiavi primarie, le relazioni e gli indici
        //____________________________________________________________________________________________
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            // 
            // MATERIAL DEFINITION 
            //

            // MaterialDefinition, chiave primaria e relazioni
            modelBuilder.Entity<MaterialDefinition>()
                .HasKey(m => m.Id);

            // Indice univoco su MaterialId
            modelBuilder.Entity<MaterialDefinition>()
                .HasIndex(m => m.MaterialId)
                .IsUnique();    // Identificatore di dominio ISA‑95

            // MaterialId è obbligatorio
            modelBuilder.Entity<MaterialDefinition>()
                .Property(m => m.MaterialId)
                .IsRequired();

            // Relazione 1:N con MaterialLot
            modelBuilder.Entity<MaterialDefinition>()
                .HasMany(m => m.Lots)
                .WithOne(l => l.MaterialDefinition)
                .HasForeignKey(l => l.MaterialDefinitionId)
                .OnDelete(DeleteBehavior.Restrict); // In un MES non si cancellano i materiali se esistono lotti





            // 
            // MATERIAL LOT 
            //

            // MaterialLot, chiave primaria e relazioni
            modelBuilder.Entity<MaterialLot>()
                .HasKey(l => l.Id);

            // Indice univoco su LotId
            modelBuilder.Entity<MaterialLot>()
                .HasIndex(l => l.LotId)
                .IsUnique();

            // LotId è obbligatorio
            modelBuilder.Entity<MaterialLot>()
                .Property(l => l.LotId)
                .IsRequired();

            // Configurazione precisione per Quantity
            modelBuilder.Entity<MaterialLot>()
                .Property(m => m.Quantity)
                .HasPrecision(18, 4);


            // 
            // WORK ORDER 
            //
            
            modelBuilder.Entity<WorkOrder>() 
                .HasKey(w => w.Id); 
            
            modelBuilder.Entity<WorkOrder>() 
                .HasIndex(w => w.WorkOrderId) 
                .IsUnique(); // Identificatore di dominio

            // WorkOrderId è obbligatorio
            modelBuilder.Entity<WorkOrder>() 
                .Property(w => w.WorkOrderId) 
                .IsRequired(); 
            
            modelBuilder.Entity<WorkOrder>() 
                .HasMany(w => w.MaterialRequirements) 
                .WithOne(m => m.WorkOrder) 
                .HasForeignKey(m => m.WorkOrderId) 
                .OnDelete(DeleteBehavior.Cascade); // Se elimini un WorkOrder, elimini i suoi requisiti


            // 
            // MATERIAL REQUIREMENT 
            //

            modelBuilder.Entity<MaterialRequirement>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<MaterialRequirement>()
                .HasOne(m => m.WorkOrder)
                .WithMany(w => w.MaterialRequirements)
                .HasForeignKey(m => m.WorkOrderId);

            // Relazione con MaterialDefinition, ma senza cascade delete
            modelBuilder.Entity<MaterialRequirement>()
                .HasOne(m => m.MaterialDefinition)
                .WithMany()
                .HasForeignKey(m => m.MaterialDefinitionId)
                .OnDelete(DeleteBehavior.Restrict); // Non si può cancellare un MaterialDefinition se usato in un WorkOrder

            // Configure precision for RequiredQuantity to avoid truncation
            modelBuilder.Entity<MaterialRequirement>()
                .Property(m => m.RequiredQuantity)
                .HasPrecision(18, 4);

            // 
            //
            // EQUIPMENT 
            //

            modelBuilder.Entity<Equipment>() 
                .HasKey(e => e.Id); 
            modelBuilder.Entity<Equipment>() 
                .HasIndex(e => e.EquipmentId) 
                .IsUnique(); 
            modelBuilder.Entity<Equipment>() 
                .Property(e => e.EquipmentId) 
                .IsRequired(); 
            
            // 
            // PERSONNEL 
            //
            
            modelBuilder.Entity<Personnel>() 
                .HasKey(p => p.Id); 
            modelBuilder.Entity<Personnel>() 
                .HasIndex(p => p.PersonId) 
                .IsUnique(); 
            modelBuilder.Entity<Personnel>() 
                .Property(p => p.PersonId) 
                .IsRequired();



        }
    }
}
