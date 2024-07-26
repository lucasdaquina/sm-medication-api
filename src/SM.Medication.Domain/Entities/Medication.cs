using System.ComponentModel.DataAnnotations;
using SM.Medication.Domain.Common;

namespace SM.Medication.Domain.Entities;

public class Medication : BaseEntity
{
    public string? Name { get; set; }

    private int _quantity;
    [Range(1, int.MaxValue)]
    public int Quantity
    {
        get { return _quantity; }
        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Quantity must be greater than 0");
            }
            _quantity = value;
        }
    }
}
