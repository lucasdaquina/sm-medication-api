using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SM.Medication.Application.Commands;

public class CreateMedicationCommand 
{
    [Required]
    public string? Name { get; set; }
    [Required]
    [DefaultValue(1)]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
