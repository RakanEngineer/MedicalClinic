namespace MedicalClinic.ManagementSystem.Domain.Models.Entities;

public class Prescription
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public Guid? MedicalRecordId { get; set; }
    public string MedicationName { get; set; } = string.Empty;
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string? Instructions { get; set; }
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = PrescriptionStatuses.Active;
    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
    public MedicalRecord? MedicalRecord { get; set; }
}
