
namespace SM.Medication.Domain.Interfaces;

public interface IMedicationRepository
{
    Task<bool> Add(Entities.Medication entity);
    Task<List<Domain.Entities.Medication>> GetAll();
    Task<Domain.Entities.Medication?> GetByName(string name);
}
