using System.ComponentModel.DataAnnotations;

namespace MedicalClinic.ManagementSystem.Shared.DTOs.Appointments;

public abstract record AppointmentManipulationDto
{
    [Required]
    public DateTime AppointmentDate { get; init; }

    [Required]
    [MaxLength(50)]
    public string Status { get; init; } = "Scheduled";

    [MaxLength(500)]
    public string? Notes { get; init; }

    [Required]
    public Guid DoctorId { get; init; }

    [Required]
    public Guid PatientId { get; init; }
}
