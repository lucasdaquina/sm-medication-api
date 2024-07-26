using SM.Medication.Application.Commands;

namespace SM.Medication.Application.Interfaces;
public interface IMedicationHandler
{
    Task<List<MedicationDTO>> Handle();
    Task<bool> Handle(CreateMedicationCommand command);
}
