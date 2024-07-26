using Mapster;
using MapsterMapper;
using SM.Medication.Application.Commands;
using SM.Medication.Application.Interfaces;
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

    public async Task<bool> Handle(CreateMedicationCommand command)
    {
        TypeAdapterConfig<CreateMedicationCommand, Domain.Entities.Medication>
            .NewConfig()
            .Map(dest => dest.CreatedBy, src =>  Environment.UserName)
            .Map(dest => dest.CreatedAt, src => DateTime.UtcNow)
            .Map(dest => dest.ModifiedBy, src => Environment.UserName)
            .Map(dest => dest.ModifiedAt, src => DateTime.UtcNow);

        //var entity = new Domain.Entities.Medication();
        var entity = mapper.Map<Domain.Entities.Medication>(command);   

        var isMedicationExist = (await medicationRepository.GetByName(entity.Name!)) is null;

        if (!isMedicationExist)
            throw new Exception("Medication already exist.");

        var result = await medicationRepository.Add(entity);
        return result > 0;
    }

    public async Task<bool> Handle(DeleteMedicationCommand command)
    {
        var result = await medicationRepository.Delete(command.Name!);
        return result > 0;
    }
}
