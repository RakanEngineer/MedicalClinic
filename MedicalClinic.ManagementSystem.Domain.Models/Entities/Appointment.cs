namespace MedicalClinic.ManagementSystem.Domain.Models.Entities;

public class Appointment
{
    public Guid Id { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } = "Scheduled";
    public string? Notes { get; set; }
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public Doctor? Doctor { get; set; }
    public Patient? Patient { get; set; }
}
