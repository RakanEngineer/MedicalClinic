namespace MedicalClinic.ManagementSystem.Service.Contracts;

public interface IServiceManager
{
    IPatientService PatientService { get; }
    IDoctorService DoctorService { get; }
    IAppointmentService AppointmentService { get; }
    IMedicalRecordService MedicalRecordService { get; }
    IPrescriptionService PrescriptionService { get; }
    IAuthService AuthService { get; }
}
