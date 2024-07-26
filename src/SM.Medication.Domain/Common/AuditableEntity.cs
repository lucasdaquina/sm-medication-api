namespace SM.Medication.Domain.Common;

public class AuditableEntity
{
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
