namespace MedicalClinic.ManagementSystem.Domain.Models.Exceptions;

public sealed class PrescriptionStatusException : BadRequestException
{
    public PrescriptionStatusException(string status)
        : base($"Prescription status '{status}' is invalid.", "Invalid Prescription Status")
    {
    }
}
