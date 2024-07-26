using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SM.Medication.Infrastructure.Mapping;
public class MedicationMapping : IEntityTypeConfiguration<SM.Medication.Domain.Entities.Medication>
{
    public void Configure(EntityTypeBuilder<SM.Medication.Domain.Entities.Medication> builder)
    {
        builder.ToTable("Medication");
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Id)
            .HasColumnName("MedicationId")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .HasColumnName("Name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Quantity)
            .HasColumnName("Quantity")
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .HasColumnName("CreatedAt")
            .IsRequired();

        builder.Property(p => p.CreatedBy)
            .HasColumnName("CreatedBy")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.ModifiedAt)
            .HasColumnName("ModifiedAt")
            .IsRequired();

        builder.Property(p => p.ModifiedBy)
            .HasColumnName("ModifiedBy")
            .HasMaxLength(100)
            .IsRequired();
    }
}
