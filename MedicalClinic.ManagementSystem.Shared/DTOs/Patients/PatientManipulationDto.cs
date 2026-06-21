using System.ComponentModel.DataAnnotations;

namespace MedicalClinic.ManagementSystem.Shared.DTOs.Patients;

public abstract record PatientManipulationDto
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; init; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; init; }

    [Required]
    [MaxLength(30)]
    public string Gender { get; init; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string PhoneNumber { get; init; } = string.Empty;

    [EmailAddress]
    [MaxLength(254)]
    public string? Email { get; init; }

    [MaxLength(250)]
    public string? Address { get; init; }
}
