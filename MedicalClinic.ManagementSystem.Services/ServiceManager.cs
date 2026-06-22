using MedicalClinic.ManagementSystem.Service.Contracts;

namespace MedicalClinic.ManagementSystem.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IPatientService> patientService;
    private readonly Lazy<IDoctorService> doctorService;
    private readonly Lazy<IAppointmentService> appointmentService;
    private readonly Lazy<IMedicalRecordService> medicalRecordService;
    private readonly Lazy<IPrescriptionService> prescriptionService;
    private readonly Lazy<IAuthService> authService;

    public ServiceManager(
        Lazy<IPatientService> patientService,
        Lazy<IDoctorService> doctorService,
        Lazy<IAppointmentService> appointmentService,
        Lazy<IMedicalRecordService> medicalRecordService,
        Lazy<IPrescriptionService> prescriptionService,
        Lazy<IAuthService> authService)
    {
        this.patientService = patientService;
        this.doctorService = doctorService;
        this.appointmentService = appointmentService;
        this.medicalRecordService = medicalRecordService;
        this.prescriptionService = prescriptionService;
        this.authService = authService;
    }

    public IPatientService PatientService => patientService.Value;
    public IDoctorService DoctorService => doctorService.Value;
    public IAppointmentService AppointmentService => appointmentService.Value;
    public IMedicalRecordService MedicalRecordService => medicalRecordService.Value;
    public IPrescriptionService PrescriptionService => prescriptionService.Value;
    public IAuthService AuthService => authService.Value;
}
