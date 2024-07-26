namespace SM.Medication.Domain.Common;

public class AuditableEntity
{
    private string? _createdBy;
    private string? _modifiedBy;
    private DateTime _createdAt;
    private DateTime _modifiedAt;

    public string? CreatedBy
    {
        get { return _createdBy; }
        set
        {
            if (string.IsNullOrEmpty(_createdBy))
                _createdBy = Environment.UserName;
        }
    }
    public string? ModifiedBy
    {
        get { return _modifiedBy; }
        set
        {
            _modifiedBy = Environment.UserName;
        }
    }
    public DateTime CreatedAt
    {
        get { return _createdAt; }
        set
        {
            if (_createdAt == DateTime.MinValue)
                _createdAt = DateTime.UtcNow;
        }
    }

    public DateTime ModifiedAt
    {
        get { return _modifiedAt; }
        set
        {
            if (_modifiedAt == DateTime.MinValue)
                _modifiedAt = DateTime.UtcNow;
        }
    }
}
