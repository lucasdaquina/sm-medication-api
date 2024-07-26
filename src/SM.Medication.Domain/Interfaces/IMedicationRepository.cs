
namespace SM.Medication.Domain.Interfaces;

public interface IMedicationRepository
{
    Task<int> Add(Entities.Medication entity);
    Task<int> Delete(string v);
    Task<List<Domain.Entities.Medication>> GetAll();
    Task<Domain.Entities.Medication?> GetByName(string name);
}
