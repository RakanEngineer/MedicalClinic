namespace MedicalClinic.ManagementSystem.Domain.Models.Exceptions;

public sealed class DoctorInactiveException : BadRequestException
{
    public DoctorInactiveException(Guid doctorId)
        : base($"Doctor '{doctorId}' is inactive and cannot perform clinical operations.", "Inactive Doctor")
    {
    }
}
