using SM.Medication.Domain.Interfaces;
using SM.Medication.Infrastructure.Persistence;

namespace SM.Medication.Infrastructure.Services.Repositories;

public class MedicationRepository(SmartMedMedicationDbContext context) : IMedicationRepository
{
    public async Task<int> Add(Domain.Entities.Medication entity)
    {
        context.Medications.Add(entity);

        return await context.SaveChangesAsync();
    }

    public async Task<int> Delete(string name)
    {
        var medication = await GetByName(name) ?? throw new Exception("Medication not found.");

        context.Medications.Remove(medication!);

        return await context.SaveChangesAsync();
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
