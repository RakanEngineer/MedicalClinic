using Microsoft.AspNetCore.Identity;

namespace MedicalClinic.ManagementSystem.Domain.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpireTime { get; set; }
}
