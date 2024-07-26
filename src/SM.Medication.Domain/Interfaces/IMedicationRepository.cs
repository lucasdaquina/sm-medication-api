namespace SM.Medication.Domain.Interfaces;

public interface IMedicationRepository
{
    Task<List<Domain.Entities.Medication>> GetAll();
}
