using System.ComponentModel.DataAnnotations;

namespace MedicalClinic.ManagementSystem.Shared.DTOs.AuthDtos;

public record UserRegistrationDto
{
    [Required]
    [MaxLength(100)]
    public string DisplayName { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    public string UserName { get; init; } = string.Empty;

    [Required]
    public string Role { get; init; } = "User";
}
