namespace MedicalClinic.ManagementSystem.Domain.Models.Exceptions;

public sealed class PatientDuplicateException : BadRequestException
{
    public PatientDuplicateException(string fieldName, string value)
        : base($"A patient with {fieldName} '{value}' already exists.", "Duplicate Patient")
    {
    }
}
