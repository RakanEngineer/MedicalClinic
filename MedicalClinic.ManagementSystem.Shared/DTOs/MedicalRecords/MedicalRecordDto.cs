namespace MedicalClinic.ManagementSystem.Shared.DTOs.MedicalRecords;

public record MedicalRecordDto
{
    public Guid Id { get; init; }
    public string Diagnosis { get; init; } = string.Empty;
    public string Treatment { get; init; } = string.Empty;
    public string? Prescription { get; init; }
    public DateTime RecordDate { get; init; }
    public Guid PatientId { get; init; }
}
