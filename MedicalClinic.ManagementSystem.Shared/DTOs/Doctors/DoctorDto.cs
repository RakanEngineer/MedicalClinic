namespace MedicalClinic.ManagementSystem.Shared.DTOs.Doctors;

public record DoctorDto
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Specialty { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string? Email { get; init; }
    public DateTime CreatedAt { get; init; }
}
