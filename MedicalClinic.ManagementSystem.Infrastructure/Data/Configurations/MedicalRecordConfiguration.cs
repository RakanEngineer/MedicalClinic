using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalClinic.ManagementSystem.Infrastructure.Data.Configurations;

public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
{
    public void Configure(EntityTypeBuilder<MedicalRecord> builder)
    {
        builder.HasKey(record => record.Id);
        builder.Property(record => record.Diagnosis).IsRequired().HasMaxLength(500);
        builder.Property(record => record.Treatment).IsRequired().HasMaxLength(500);
        builder.Property(record => record.Prescription).HasMaxLength(1000);
        builder.Property(record => record.RecordDate).IsRequired();

        builder.HasOne(record => record.Patient)
            .WithMany(patient => patient.MedicalRecords)
            .HasForeignKey(record => record.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
