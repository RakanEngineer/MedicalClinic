namespace MedicalClinic.ManagementSystem.Domain.Models.Exceptions;

public sealed class AppointmentStatusException : BadRequestException
{
    public AppointmentStatusException(string status)
        : base($"Appointment status '{status}' is invalid.", "Invalid Appointment Status")
    {
    }
}
