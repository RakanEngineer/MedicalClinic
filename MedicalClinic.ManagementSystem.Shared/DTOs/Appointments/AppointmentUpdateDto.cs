namespace MedicalClinic.ManagementSystem.Shared.DTOs.Appointments;

public sealed record AppointmentUpdateDto : AppointmentManipulationDto
{
    public Guid Id { get; init; }
}
