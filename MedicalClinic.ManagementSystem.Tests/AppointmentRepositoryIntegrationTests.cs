using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Infrastructure.Data;
using MedicalClinic.ManagementSystem.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinic.ManagementSystem.Tests;

public class AppointmentRepositoryIntegrationTests
{
    [Fact]
    public async Task HasOverlappingAppointmentAsync_WhenRequestedTimeOverlapsExisting_ReturnsTrue()
    {
        await using var context = await CreateContextAsync();
        var (doctorId, patientId) = await SeedDoctorAndPatientAsync(context);
        context.Appointments.Add(new Appointment
        {
            Id = Guid.NewGuid(),
            DoctorId = doctorId,
            PatientId = patientId,
            AppointmentDate = new DateTime(2026, 7, 1, 10, 0, 0, DateTimeKind.Utc),
            DurationMinutes = 60,
            Status = AppointmentStatuses.Scheduled
        });
        await context.SaveChangesAsync();

        var repository = new AppointmentRepository(context);

        bool hasOverlap = await repository.HasOverlappingAppointmentAsync(
            doctorId,
            new DateTime(2026, 7, 1, 10, 30, 0, DateTimeKind.Utc),
            30);

        Assert.True(hasOverlap);
    }

    [Fact]
    public async Task HasOverlappingAppointmentAsync_WhenRequestedTimeIsBackToBack_ReturnsFalse()
    {
        await using var context = await CreateContextAsync();
        var (doctorId, patientId) = await SeedDoctorAndPatientAsync(context);
        context.Appointments.Add(new Appointment
        {
            Id = Guid.NewGuid(),
            DoctorId = doctorId,
            PatientId = patientId,
            AppointmentDate = new DateTime(2026, 7, 1, 10, 0, 0, DateTimeKind.Utc),
            DurationMinutes = 60,
            Status = AppointmentStatuses.Scheduled
        });
        await context.SaveChangesAsync();

        var repository = new AppointmentRepository(context);

        bool hasOverlap = await repository.HasOverlappingAppointmentAsync(
            doctorId,
            new DateTime(2026, 7, 1, 11, 0, 0, DateTimeKind.Utc),
            30);

        Assert.False(hasOverlap);
    }

    private static async Task<ApplicationDbContext> CreateContextAsync()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ApplicationDbContext(options);
        await context.Database.EnsureCreatedAsync();

        return context;
    }

    private static async Task<(Guid DoctorId, Guid PatientId)> SeedDoctorAndPatientAsync(ApplicationDbContext context)
    {
        var doctorId = Guid.NewGuid();
        var patientId = Guid.NewGuid();

        context.Doctors.Add(new Doctor
        {
            Id = doctorId,
            FirstName = "Mina",
            LastName = "Karim",
            Specialty = "Cardiology",
            PhoneNumber = "+9647700000001",
            IsActive = true
        });
        context.Patients.Add(new Patient
        {
            Id = patientId,
            FirstName = "Amal",
            LastName = "Hassan",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = "Female",
            PhoneNumber = "+9647700000000"
        });
        await context.SaveChangesAsync();

        return (doctorId, patientId);
    }
}
