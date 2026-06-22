using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;
using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Infrastructure.Data;
using MedicalClinic.ManagementSystem.Shared.Request;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinic.ManagementSystem.Infrastructure.Repositories;

public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Appointment?> GetAppointmentAsync(Guid id, bool trackChanges = false) =>
        await FindByCondition(appointment => appointment.Id == id, trackChanges).FirstOrDefaultAsync();

    public async Task<bool> HasOverlappingAppointmentAsync(Guid doctorId, DateTime appointmentDate, int durationMinutes, Guid? excludedAppointmentId = null)
    {
        DateTime requestedEnd = appointmentDate.AddMinutes(durationMinutes);

        IQueryable<Appointment> appointments = FindByCondition(appointment =>
            appointment.DoctorId == doctorId &&
            appointment.Status != AppointmentStatuses.Cancelled &&
            appointment.AppointmentDate < requestedEnd &&
            appointment.AppointmentDate.AddMinutes(appointment.DurationMinutes) > appointmentDate);

        if (excludedAppointmentId.HasValue)
            appointments = appointments.Where(appointment => appointment.Id != excludedAppointmentId.Value);

        return await appointments.AnyAsync();
    }

    public async Task<PagedList<Appointment>> GetAppointmentsAsync(AppointmentRequestParams requestParams, bool trackChanges = false)
    {
        IQueryable<Appointment> appointments = FindAll(trackChanges).OrderBy(appointment => appointment.AppointmentDate);

        if (requestParams.PatientId.HasValue)
            appointments = appointments.Where(appointment => appointment.PatientId == requestParams.PatientId.Value);

        if (requestParams.DoctorId.HasValue)
            appointments = appointments.Where(appointment => appointment.DoctorId == requestParams.DoctorId.Value);

        if (requestParams.FromUtc.HasValue)
            appointments = appointments.Where(appointment => appointment.AppointmentDate >= requestParams.FromUtc.Value);

        if (requestParams.ToUtc.HasValue)
            appointments = appointments.Where(appointment => appointment.AppointmentDate <= requestParams.ToUtc.Value);

        if (!string.IsNullOrWhiteSpace(requestParams.Status))
        {
            string status = requestParams.Status.Trim();
            appointments = appointments.Where(appointment => appointment.Status.Contains(status));
        }

        return await appointments.ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);
    }
}
