namespace MedicalClinic.ManagementSystem.Shared.DTOs.Prescriptions;

public sealed record UpdatePrescriptionDto : PrescriptionManipulationDto
{
    public Guid Id { get; init; }
}
