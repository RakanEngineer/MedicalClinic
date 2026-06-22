using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Shared.Request;

namespace MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;

public interface IAppointmentRepository : IRepositoryBase<Appointment>
{
    Task<PagedList<Appointment>> GetAppointmentsAsync(AppointmentRequestParams requestParams, bool trackChanges = false);
    Task<Appointment?> GetAppointmentAsync(Guid id, bool trackChanges = false);
    Task<bool> HasOverlappingAppointmentAsync(Guid doctorId, DateTime appointmentDate, int durationMinutes, Guid? excludedAppointmentId = null);
}
