using System.ComponentModel.DataAnnotations;

namespace MedicalClinic.ManagementSystem.Shared.DTOs.Appointments;

public sealed record AppointmentRescheduleDto
{
    [Required]
    public DateTime AppointmentDate { get; init; }

    [Range(5, 480)]
    public int DurationMinutes { get; init; } = 30;
}
