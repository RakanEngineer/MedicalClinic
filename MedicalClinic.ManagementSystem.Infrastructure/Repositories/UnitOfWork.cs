using MedicalClinic.ManagementSystem.Infrastructure.Data;
using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;

namespace MedicalClinic.ManagementSystem.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly Lazy<IPatientRepository> patientRepository;
    private readonly Lazy<IDoctorRepository> doctorRepository;
    private readonly Lazy<IAppointmentRepository> appointmentRepository;
    private readonly Lazy<IMedicalRecordRepository> medicalRecordRepository;
    private readonly Lazy<IPrescriptionRepository> prescriptionRepository;
    private readonly ApplicationDbContext context;

    public UnitOfWork(
        ApplicationDbContext context,
        Lazy<IPatientRepository> patientRepository,
        Lazy<IDoctorRepository> doctorRepository,
        Lazy<IAppointmentRepository> appointmentRepository,
        Lazy<IMedicalRecordRepository> medicalRecordRepository,
        Lazy<IPrescriptionRepository> prescriptionRepository)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        this.doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
        this.appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        this.medicalRecordRepository = medicalRecordRepository ?? throw new ArgumentNullException(nameof(medicalRecordRepository));
        this.prescriptionRepository = prescriptionRepository ?? throw new ArgumentNullException(nameof(prescriptionRepository));
    }

    public IPatientRepository PatientRepository => patientRepository.Value;
    public IDoctorRepository DoctorRepository => doctorRepository.Value;
    public IAppointmentRepository AppointmentRepository => appointmentRepository.Value;
    public IMedicalRecordRepository MedicalRecordRepository => medicalRecordRepository.Value;
    public IPrescriptionRepository PrescriptionRepository => prescriptionRepository.Value;

    public async Task CompleteAsync() => await context.SaveChangesAsync();
}
