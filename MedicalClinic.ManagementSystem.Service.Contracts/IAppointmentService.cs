using MedicalClinic.ManagementSystem.Shared.DTOs.Appointments;
using MedicalClinic.ManagementSystem.Shared.Request;

namespace MedicalClinic.ManagementSystem.Service.Contracts;

public interface IAppointmentService
{
    Task<(IEnumerable<AppointmentDto> Items, MetaData MetaData)> GetAppointmentsAsync(AppointmentRequestParams requestParams, bool trackChanges = false);
    Task<AppointmentDto> GetAppointmentAsync(Guid id, bool trackChanges = false);
    Task<AppointmentDto> CreateAppointmentAsync(AppointmentCreateDto dto);
    Task UpdateAppointmentAsync(Guid id, AppointmentUpdateDto dto);
    Task RescheduleAppointmentAsync(Guid id, AppointmentRescheduleDto dto);
    Task CancelAppointmentAsync(Guid id);
    Task DeleteAppointmentAsync(Guid id);
}
