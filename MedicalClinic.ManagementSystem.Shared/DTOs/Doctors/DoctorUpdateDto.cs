namespace MedicalClinic.ManagementSystem.Shared.DTOs.Doctors;

public sealed record DoctorUpdateDto : DoctorManipulationDto
{
    public Guid Id { get; init; }
}
