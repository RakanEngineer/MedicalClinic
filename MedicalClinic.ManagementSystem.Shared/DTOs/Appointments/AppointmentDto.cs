namespace MedicalClinic.ManagementSystem.Shared.DTOs.Appointments;

public record AppointmentDto
{
    public Guid Id { get; init; }
    public DateTime AppointmentDate { get; init; }
    public int DurationMinutes { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Notes { get; init; }
    public Guid DoctorId { get; init; }
    public Guid PatientId { get; init; }
}
