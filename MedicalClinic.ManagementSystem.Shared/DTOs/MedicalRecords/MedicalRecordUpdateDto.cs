namespace MedicalClinic.ManagementSystem.Shared.DTOs.MedicalRecords;

public sealed record MedicalRecordUpdateDto : MedicalRecordManipulationDto
{
    public Guid Id { get; init; }
}
