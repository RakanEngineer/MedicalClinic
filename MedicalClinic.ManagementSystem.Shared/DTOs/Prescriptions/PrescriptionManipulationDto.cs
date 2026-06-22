using System.ComponentModel.DataAnnotations;

namespace MedicalClinic.ManagementSystem.Shared.DTOs.Prescriptions;

public abstract record PrescriptionManipulationDto
{
    [Required]
    public Guid PatientId { get; init; }

    [Required]
    public Guid DoctorId { get; init; }

    public Guid? MedicalRecordId { get; init; }

    [Required]
    [MaxLength(150)]
    public string MedicationName { get; init; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Dosage { get; init; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Frequency { get; init; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Duration { get; init; } = string.Empty;

    [MaxLength(1000)]
    public string? Instructions { get; init; }

    public DateTime? IssuedAt { get; init; }

    [Required]
    [MaxLength(50)]
    public string Status { get; init; } = "Active";
}
