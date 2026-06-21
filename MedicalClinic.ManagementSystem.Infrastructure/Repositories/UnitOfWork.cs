using MedicalClinic.ManagementSystem.Infrastructure.Data;
using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;

namespace MedicalClinic.ManagementSystem.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly Lazy<IPatientRepository> patientRepository;
    private readonly Lazy<IDoctorRepository> doctorRepository;
    private readonly Lazy<IAppointmentRepository> appointmentRepository;
    private readonly Lazy<IMedicalRecordRepository> medicalRecordRepository;
    private readonly ApplicationDbContext context;

    public UnitOfWork(
        ApplicationDbContext context,
        Lazy<IPatientRepository> patientRepository,
        Lazy<IDoctorRepository> doctorRepository,
        Lazy<IAppointmentRepository> appointmentRepository,
        Lazy<IMedicalRecordRepository> medicalRecordRepository)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        this.doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
        this.appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        this.medicalRecordRepository = medicalRecordRepository ?? throw new ArgumentNullException(nameof(medicalRecordRepository));
    }

    public IPatientRepository PatientRepository => patientRepository.Value;
    public IDoctorRepository DoctorRepository => doctorRepository.Value;
    public IAppointmentRepository AppointmentRepository => appointmentRepository.Value;
    public IMedicalRecordRepository MedicalRecordRepository => medicalRecordRepository.Value;

    public async Task CompleteAsync() => await context.SaveChangesAsync();
}
