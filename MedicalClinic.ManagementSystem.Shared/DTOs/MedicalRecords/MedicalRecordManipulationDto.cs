using System.ComponentModel.DataAnnotations;

namespace MedicalClinic.ManagementSystem.Shared.DTOs.MedicalRecords;

public abstract record MedicalRecordManipulationDto
{
    [Required]
    [MaxLength(500)]
    public string Diagnosis { get; init; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Treatment { get; init; } = string.Empty;

    [MaxLength(1000)]
    public string? Prescription { get; init; }

    [Required]
    public DateTime RecordDate { get; init; } = DateTime.UtcNow;

    [Required]
    public Guid PatientId { get; init; }
}
