namespace MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;

public interface IUnitOfWork
{
    IPatientRepository PatientRepository { get; }
    IDoctorRepository DoctorRepository { get; }
    IAppointmentRepository AppointmentRepository { get; }
    IMedicalRecordRepository MedicalRecordRepository { get; }
    IPrescriptionRepository PrescriptionRepository { get; }

    Task CompleteAsync();
}
