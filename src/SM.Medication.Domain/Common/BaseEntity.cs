namespace SM.Medication.Domain.Common;

public class BaseEntity : AuditableEntity
{
    public int Id { get; private set; }
}
