namespace MedicalClinic.ManagementSystem.Shared.Authorization;

public static class AuthorizationPolicies
{
    public const string AdminOnly = "AdminOnly";
    public const string CanWrite = "CanWrite";
    public const string AuthenticatedUser = "AuthenticatedUser";
}
