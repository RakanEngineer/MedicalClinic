namespace MedicalClinic.ManagementSystem.Domain.Models.Exceptions;

public sealed class AppointmentOverlapException : BadRequestException
{
    public AppointmentOverlapException(Guid doctorId, DateTime appointmentDate)
        : base($"Doctor '{doctorId}' already has an appointment at '{appointmentDate:O}'.", "Appointment Overlap")
    {
    }
}
