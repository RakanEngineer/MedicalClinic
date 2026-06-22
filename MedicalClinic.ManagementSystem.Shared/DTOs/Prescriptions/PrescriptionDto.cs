namespace MedicalClinic.ManagementSystem.Shared.DTOs.Prescriptions;

public record PrescriptionDto
{
    public Guid Id { get; init; }
    public Guid PatientId { get; init; }
    public Guid DoctorId { get; init; }
    public Guid? MedicalRecordId { get; init; }
    public string MedicationName { get; init; } = string.Empty;
    public string Dosage { get; init; } = string.Empty;
    public string Frequency { get; init; } = string.Empty;
    public string Duration { get; init; } = string.Empty;
    public string? Instructions { get; init; }
    public DateTime IssuedAt { get; init; }
    public string Status { get; init; } = string.Empty;
}
