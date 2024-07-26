using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
            new SM.Medication.Domain.Entities.Medication { Name = "Paracetamol", Quantity = 100, CreatedAt = DateTime.UtcNow, CreatedBy = "DbContext", ModifiedAt = DateTime.UtcNow, ModifiedBy = "DbContex" },
            new SM.Medication.Domain.Entities.Medication { Name = "Ibuprofen", Quantity = 50, CreatedAt = DateTime.UtcNow, CreatedBy = "DbContext", ModifiedAt = DateTime.UtcNow, ModifiedBy = "DbContex" },
            new SM.Medication.Domain.Entities.Medication { Name = "Aspirin", Quantity = 75, CreatedAt = DateTime.UtcNow, CreatedBy = "DbContext", ModifiedAt = DateTime.UtcNow, ModifiedBy = "DbContex" });

    }
}
