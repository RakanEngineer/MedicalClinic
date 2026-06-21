namespace MedicalClinic.ManagementSystem.Domain.Models.Entities;

public class MedicalRecord
{
    public Guid Id { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string? Prescription { get; set; }
    public DateTime RecordDate { get; set; } = DateTime.UtcNow;
    public Guid PatientId { get; set; }
    public Patient? Patient { get; set; }
}
