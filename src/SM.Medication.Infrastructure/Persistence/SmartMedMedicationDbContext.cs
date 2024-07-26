using Microsoft.EntityFrameworkCore.Storage;

namespace SM.Medication.Infrastructure.Persistence;

public class SmartMedMedicationDbContext : DbContext
{
    public SmartMedMedicationDbContext(DbContextOptions<SmartMedMedicationDbContext> options) : base(options)
    {
        var dbCreater = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;

        if (dbCreater != null)
        {
            if (!dbCreater.CanConnect())
            {
                dbCreater.Create();
            }

            if (!dbCreater.HasTables())
            {
                dbCreater.CreateTables();

            }
        }
    }

    public DbSet<SM.Medication.Domain.Entities.Medication> Medications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SmartMedMedicationDbContext).Assembly);

        modelBuilder.Entity<SM.Medication.Domain.Entities.Medication>().HasData(
            new SM.Medication.Domain.Entities.Medication { Id = 1, Name = "Paracetamol", Quantity = 100, CreatedAt = DateTime.UtcNow, CreatedBy = "DbContext", ModifiedAt = DateTime.UtcNow, ModifiedBy = "DbContex" },
            new SM.Medication.Domain.Entities.Medication { Id = 2, Name = "Ibuprofen", Quantity = 50, CreatedAt = DateTime.UtcNow, CreatedBy = "DbContext", ModifiedAt = DateTime.UtcNow, ModifiedBy = "DbContex" },
            new SM.Medication.Domain.Entities.Medication { Id = 3, Name = "Aspirin", Quantity = 75, CreatedAt = DateTime.UtcNow, CreatedBy = "DbContext", ModifiedAt = DateTime.UtcNow, ModifiedBy = "DbContex" });

    }
}
