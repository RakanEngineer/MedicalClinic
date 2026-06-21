using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalClinic.ManagementSystem.Infrastructure.Data.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(patient => patient.Id);
        builder.Property(patient => patient.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(patient => patient.LastName).IsRequired().HasMaxLength(100);
        builder.Property(patient => patient.Gender).IsRequired().HasMaxLength(30);
        builder.Property(patient => patient.PhoneNumber).IsRequired().HasMaxLength(30);
        builder.Property(patient => patient.Email).HasMaxLength(254);
        builder.Property(patient => patient.Address).HasMaxLength(250);
        builder.Property(patient => patient.DateOfBirth).IsRequired();
        builder.Property(patient => patient.CreatedAt).IsRequired();
    }
}
