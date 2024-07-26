using SM.Medication.Domain.Interfaces;
using SM.Medication.Infrastructure.Persistence;

namespace SM.Medication.Infrastructure.Services.Repositories;

public class MedicationRepository(SmartMedMedicationDbContext context) : IMedicationRepository
{
    public async Task<List<Domain.Entities.Medication>> GetAll()
    {
        return await context
            .Medications
            .ToListAsync();
    }
}
