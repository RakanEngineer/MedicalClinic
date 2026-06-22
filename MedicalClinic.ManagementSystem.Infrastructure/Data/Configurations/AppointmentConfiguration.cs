using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalClinic.ManagementSystem.Infrastructure.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable(table =>
        {
            table.HasCheckConstraint(
                "CK_Appointments_Status",
                "[Status] IN ('Scheduled', 'Completed', 'Cancelled', 'NoShow')");
        });

        builder.HasKey(appointment => appointment.Id);
        builder.Property(appointment => appointment.AppointmentDate).IsRequired();
        builder.Property(appointment => appointment.DurationMinutes).IsRequired();
        builder.Property(appointment => appointment.Status).IsRequired().HasMaxLength(50);
        builder.Property(appointment => appointment.Notes).HasMaxLength(500);

        builder.HasOne(appointment => appointment.Patient)
            .WithMany(patient => patient.Appointments)
            .HasForeignKey(appointment => appointment.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(appointment => appointment.Doctor)
            .WithMany(doctor => doctor.Appointments)
            .HasForeignKey(appointment => appointment.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(appointment => new { appointment.DoctorId, appointment.AppointmentDate })
            .HasDatabaseName("IX_Appointments_DoctorId_AppointmentDate");

        builder.HasIndex(appointment => new { appointment.PatientId, appointment.AppointmentDate })
            .HasDatabaseName("IX_Appointments_PatientId_AppointmentDate");
    }
}
