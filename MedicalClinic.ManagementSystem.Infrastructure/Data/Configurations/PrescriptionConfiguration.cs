using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalClinic.ManagementSystem.Infrastructure.Data.Configurations;

public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "CK_Prescriptions_Status",
                "[Status] IN ('Active', 'Completed', 'Cancelled')");
        });

        builder.HasKey(prescription => prescription.Id);
        builder.Property(prescription => prescription.MedicationName).IsRequired().HasMaxLength(150);
        builder.Property(prescription => prescription.Dosage).IsRequired().HasMaxLength(100);
        builder.Property(prescription => prescription.Frequency).IsRequired().HasMaxLength(100);
        builder.Property(prescription => prescription.Duration).IsRequired().HasMaxLength(100);
        builder.Property(prescription => prescription.Instructions).HasMaxLength(1000);
        builder.Property(prescription => prescription.IssuedAt).IsRequired();
        builder.Property(prescription => prescription.Status).IsRequired().HasMaxLength(50);

        builder.HasOne(prescription => prescription.Patient)
            .WithMany(patient => patient.Prescriptions)
            .HasForeignKey(prescription => prescription.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(prescription => prescription.Doctor)
            .WithMany(doctor => doctor.Prescriptions)
            .HasForeignKey(prescription => prescription.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(prescription => prescription.MedicalRecord)
            .WithMany(record => record.Prescriptions)
            .HasForeignKey(prescription => prescription.MedicalRecordId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(prescription => prescription.PatientId)
            .HasDatabaseName("IX_Prescriptions_PatientId");
        builder.HasIndex(prescription => prescription.DoctorId)
            .HasDatabaseName("IX_Prescriptions_DoctorId");
        builder.HasIndex(prescription => prescription.MedicalRecordId)
            .HasDatabaseName("IX_Prescriptions_MedicalRecordId");
        builder.HasIndex(prescription => prescription.IssuedAt)
            .HasDatabaseName("IX_Prescriptions_IssuedAt");
        builder.HasIndex(prescription => prescription.Status)
            .HasDatabaseName("IX_Prescriptions_Status");
    }
}
