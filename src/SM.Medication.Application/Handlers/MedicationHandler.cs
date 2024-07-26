using MapsterMapper;
using SM.Medication.Application.Interfaces;
using SM.Medication.Domain.DTO;
using SM.Medication.Domain.Interfaces;

namespace SM.Medication.Application.Handlers;

public class MedicationHandler(
    IMedicationRepository medicationRepository,
    IMapper mapper) : IMedicationHandler
{
    public async Task<List<MedicationDTO>> Handle()
    {
        var medications = await medicationRepository.GetAll();
        return mapper.Map<List<MedicationDTO>>(medications); 
    }
}
