namespace SM.Medication.Application.Interfaces;
public interface IMedicationHandler
{
    Task<List<MedicationDTO>> Handle();
}
