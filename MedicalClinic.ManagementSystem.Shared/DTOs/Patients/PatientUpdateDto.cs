namespace MedicalClinic.ManagementSystem.Shared.DTOs.Patients;

public sealed record PatientUpdateDto : PatientManipulationDto
{
    public Guid Id { get; init; }
}
