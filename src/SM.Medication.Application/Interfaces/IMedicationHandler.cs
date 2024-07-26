using SM.Medication.Domain.DTO;

namespace SM.Medication.Application.Interfaces;
public interface IMedicationHandler
{
    Task<List<MedicationDTO>> Handle();
}
