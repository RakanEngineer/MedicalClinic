using AutoMapper;
using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;
using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Domain.Models.Exceptions;
using MedicalClinic.ManagementSystem.Service.Contracts;
using MedicalClinic.ManagementSystem.Shared.DTOs.Appointments;
using MedicalClinic.ManagementSystem.Shared.Request;

namespace MedicalClinic.ManagementSystem.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<(IEnumerable<AppointmentDto> Items, MetaData MetaData)> GetAppointmentsAsync(AppointmentRequestParams requestParams, bool trackChanges = false)
    {
        var pagedList = await unitOfWork.AppointmentRepository.GetAppointmentsAsync(requestParams, trackChanges);
        return (mapper.Map<IEnumerable<AppointmentDto>>(pagedList.Items), pagedList.MetaData);
    }

    public async Task<AppointmentDto> GetAppointmentAsync(Guid id, bool trackChanges = false)
    {
        var appointment = await unitOfWork.AppointmentRepository.GetAppointmentAsync(id, trackChanges);
        if (appointment is null) throw new EntityNotFoundException(nameof(Appointment), id);
        return mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> CreateAppointmentAsync(AppointmentCreateDto dto)
    {
        EnsureValidStatus(dto.Status);
        await EnsureDoctorIsActiveAsync(dto.DoctorId);
        await EnsureDoctorIsAvailableAsync(dto.DoctorId, dto.AppointmentDate, dto.DurationMinutes);

        var appointment = mapper.Map<Appointment>(dto);
        unitOfWork.AppointmentRepository.Create(appointment);
        await unitOfWork.CompleteAsync();
        return mapper.Map<AppointmentDto>(appointment);
    }

    public async Task UpdateAppointmentAsync(Guid id, AppointmentUpdateDto dto)
    {
        if (id != dto.Id) throw new EntityIdMismatchException(nameof(Appointment), id, dto.Id);
        var appointment = await unitOfWork.AppointmentRepository.GetAppointmentAsync(id, trackChanges: true);
        if (appointment is null) throw new EntityNotFoundException(nameof(Appointment), id);

        EnsureValidStatus(dto.Status);
        await EnsureDoctorIsAvailableAsync(dto.DoctorId, dto.AppointmentDate, dto.DurationMinutes, id);

        mapper.Map(dto, appointment);
        await unitOfWork.CompleteAsync();
    }

    public async Task RescheduleAppointmentAsync(Guid id, AppointmentRescheduleDto dto)
    {
        var appointment = await unitOfWork.AppointmentRepository.GetAppointmentAsync(id, trackChanges: true);
        if (appointment is null) throw new EntityNotFoundException(nameof(Appointment), id);

        await EnsureDoctorIsActiveAsync(appointment.DoctorId);
        await EnsureDoctorIsAvailableAsync(appointment.DoctorId, dto.AppointmentDate, dto.DurationMinutes, id);

        appointment.AppointmentDate = dto.AppointmentDate;
        appointment.DurationMinutes = dto.DurationMinutes;
        appointment.Status = AppointmentStatuses.Scheduled;
        await unitOfWork.CompleteAsync();
    }

    public async Task CancelAppointmentAsync(Guid id)
    {
        var appointment = await unitOfWork.AppointmentRepository.GetAppointmentAsync(id, trackChanges: true);
        if (appointment is null) throw new EntityNotFoundException(nameof(Appointment), id);

        appointment.Status = AppointmentStatuses.Cancelled;
        await unitOfWork.CompleteAsync();
    }

    public async Task DeleteAppointmentAsync(Guid id)
    {
        var appointment = await unitOfWork.AppointmentRepository.GetAppointmentAsync(id);
        if (appointment is null) throw new EntityNotFoundException(nameof(Appointment), id);
        unitOfWork.AppointmentRepository.Delete(appointment);
        await unitOfWork.CompleteAsync();
    }

    private async Task EnsureDoctorIsAvailableAsync(Guid doctorId, DateTime appointmentDate, int durationMinutes, Guid? excludedAppointmentId = null)
    {
        bool hasOverlap = await unitOfWork.AppointmentRepository.HasOverlappingAppointmentAsync(doctorId, appointmentDate, durationMinutes, excludedAppointmentId);
        if (hasOverlap) throw new AppointmentOverlapException(doctorId, appointmentDate);
    }

    private async Task EnsureDoctorIsActiveAsync(Guid doctorId)
    {
        var doctor = await unitOfWork.DoctorRepository.GetDoctorAsync(doctorId);

        if (doctor is null) throw new EntityNotFoundException(nameof(Doctor), doctorId);
        if (!doctor.IsActive) throw new DoctorInactiveException(doctorId);
    }

    private static void EnsureValidStatus(string status)
    {
        if (!AppointmentStatuses.All.Contains(status))
            throw new AppointmentStatusException(status);
    }
}
