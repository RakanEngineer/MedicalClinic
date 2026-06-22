namespace MedicalClinic.ManagementSystem.Domain.Models.Entities;

public static class PrescriptionStatuses
{
    public const string Active = "Active";
    public const string Completed = "Completed";
    public const string Cancelled = "Cancelled";

    public static readonly string[] All = [Active, Completed, Cancelled];
}
