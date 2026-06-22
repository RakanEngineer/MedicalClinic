using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalClinic.ManagementSystem.Infrastructure.Data.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(doctor => doctor.Id);
        builder.Property(doctor => doctor.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(doctor => doctor.LastName).IsRequired().HasMaxLength(100);
        builder.Property(doctor => doctor.Specialty).IsRequired().HasMaxLength(100);
        builder.Property(doctor => doctor.PhoneNumber).IsRequired().HasMaxLength(30);
        builder.Property(doctor => doctor.Email).HasMaxLength(254);
        builder.Property(doctor => doctor.IsActive).IsRequired();
        builder.Property(doctor => doctor.CreatedAt).IsRequired();

        builder.HasIndex(doctor => doctor.Specialty)
            .HasDatabaseName("IX_Doctors_Specialty");
    }
}
