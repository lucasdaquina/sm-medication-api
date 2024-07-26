using SM.Medication.Domain.Interfaces;
using SM.Medication.Infrastructure.Persistence;

namespace SM.Medication.Infrastructure.Services.Repositories;

public class MedicationRepository(SmartMedMedicationDbContext context) : IMedicationRepository
{
    public async Task<bool> Add(Domain.Entities.Medication entity)
    {
        context.Medications.Add(entity);

        var result = await context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<List<Domain.Entities.Medication>> GetAll()
    {
        return await context
            .Medications
            .ToListAsync();
    }

    public async Task<Domain.Entities.Medication?> GetByName(string name)
    {
        return await context
             .Medications
             .FirstOrDefaultAsync(med => string.Equals(med.Name!.ToUpper(), name.ToUpper()));
    }
}
