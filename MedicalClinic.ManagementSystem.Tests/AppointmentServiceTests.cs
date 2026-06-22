using AutoMapper;
using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;
using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Domain.Models.Exceptions;
using MedicalClinic.ManagementSystem.Services;
using MedicalClinic.ManagementSystem.Shared.DTOs.Appointments;
using MedicalClinic.ManagementSystem.Tests.Helpers;
using Moq;

namespace MedicalClinic.ManagementSystem.Tests;

public class AppointmentServiceTests
{
    private readonly IMapper mapper = MapperFactory.Create();
    private readonly Mock<IUnitOfWork> unitOfWork = new();
    private readonly Mock<IAppointmentRepository> appointmentRepository = new();
    private readonly Mock<IDoctorRepository> doctorRepository = new();

    public AppointmentServiceTests()
    {
        unitOfWork.SetupGet(x => x.AppointmentRepository).Returns(appointmentRepository.Object);
        unitOfWork.SetupGet(x => x.DoctorRepository).Returns(doctorRepository.Object);
    }

    [Fact]
    public async Task CreateAppointmentAsync_WhenDoctorHasAppointmentAtSameTime_ThrowsOverlap()
    {
        var doctorId = Guid.NewGuid();
        var appointmentDate = new DateTime(2026, 7, 1, 10, 0, 0, DateTimeKind.Utc);
        var service = new AppointmentService(unitOfWork.Object, mapper);
        var dto = new AppointmentCreateDto
        {
            DoctorId = doctorId,
            PatientId = Guid.NewGuid(),
            AppointmentDate = appointmentDate,
            DurationMinutes = 45,
            Status = AppointmentStatuses.Scheduled
        };

        appointmentRepository
            .Setup(x => x.HasOverlappingAppointmentAsync(doctorId, appointmentDate, 45, null))
            .ReturnsAsync(true);
        doctorRepository
            .Setup(x => x.GetDoctorAsync(doctorId, false))
            .ReturnsAsync(new Doctor { Id = doctorId, IsActive = true });

        await Assert.ThrowsAsync<AppointmentOverlapException>(() => service.CreateAppointmentAsync(dto));
        appointmentRepository.Verify(x => x.Create(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public async Task CreateAppointmentAsync_WhenDoctorIsInactive_ThrowsInactiveDoctor()
    {
        var doctorId = Guid.NewGuid();
        var service = new AppointmentService(unitOfWork.Object, mapper);
        var dto = new AppointmentCreateDto
        {
            DoctorId = doctorId,
            PatientId = Guid.NewGuid(),
            AppointmentDate = new DateTime(2026, 7, 1, 10, 0, 0, DateTimeKind.Utc),
            DurationMinutes = 30,
            Status = AppointmentStatuses.Scheduled
        };

        doctorRepository
            .Setup(x => x.GetDoctorAsync(doctorId, false))
            .ReturnsAsync(new Doctor { Id = doctorId, IsActive = false });

        await Assert.ThrowsAsync<DoctorInactiveException>(() => service.CreateAppointmentAsync(dto));
        appointmentRepository.Verify(x => x.HasOverlappingAppointmentAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<Guid?>()), Times.Never);
        appointmentRepository.Verify(x => x.Create(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public async Task CreateAppointmentAsync_WhenStatusIsInvalid_ThrowsStatusException()
    {
        var service = new AppointmentService(unitOfWork.Object, mapper);
        var dto = new AppointmentCreateDto
        {
            DoctorId = Guid.NewGuid(),
            PatientId = Guid.NewGuid(),
            AppointmentDate = new DateTime(2026, 7, 1, 10, 0, 0, DateTimeKind.Utc),
            DurationMinutes = 30,
            Status = "Waiting"
        };

        await Assert.ThrowsAsync<AppointmentStatusException>(() => service.CreateAppointmentAsync(dto));
        appointmentRepository.Verify(x => x.HasOverlappingAppointmentAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<Guid?>()), Times.Never);
    }

    [Fact]
    public async Task CancelAppointmentAsync_WhenAppointmentExists_SetsStatusToCancelled()
    {
        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            DoctorId = Guid.NewGuid(),
            PatientId = Guid.NewGuid(),
            AppointmentDate = new DateTime(2026, 7, 1, 10, 0, 0, DateTimeKind.Utc),
            DurationMinutes = 30,
            Status = AppointmentStatuses.Scheduled
        };

        appointmentRepository
            .Setup(x => x.GetAppointmentAsync(appointment.Id, true))
            .ReturnsAsync(appointment);

        var service = new AppointmentService(unitOfWork.Object, mapper);

        await service.CancelAppointmentAsync(appointment.Id);

        Assert.Equal(AppointmentStatuses.Cancelled, appointment.Status);
        unitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task RescheduleAppointmentAsync_WhenDoctorIsAvailable_UpdatesDateDurationAndStatus()
    {
        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            DoctorId = Guid.NewGuid(),
            PatientId = Guid.NewGuid(),
            AppointmentDate = new DateTime(2026, 7, 1, 10, 0, 0, DateTimeKind.Utc),
            DurationMinutes = 30,
            Status = AppointmentStatuses.Cancelled
        };
        var dto = new AppointmentRescheduleDto
        {
            AppointmentDate = new DateTime(2026, 7, 1, 11, 0, 0, DateTimeKind.Utc),
            DurationMinutes = 60
        };

        appointmentRepository
            .Setup(x => x.GetAppointmentAsync(appointment.Id, true))
            .ReturnsAsync(appointment);
        doctorRepository
            .Setup(x => x.GetDoctorAsync(appointment.DoctorId, false))
            .ReturnsAsync(new Doctor { Id = appointment.DoctorId, IsActive = true });
        appointmentRepository
            .Setup(x => x.HasOverlappingAppointmentAsync(appointment.DoctorId, dto.AppointmentDate, dto.DurationMinutes, appointment.Id))
            .ReturnsAsync(false);

        var service = new AppointmentService(unitOfWork.Object, mapper);

        await service.RescheduleAppointmentAsync(appointment.Id, dto);

        Assert.Equal(dto.AppointmentDate, appointment.AppointmentDate);
        Assert.Equal(dto.DurationMinutes, appointment.DurationMinutes);
        Assert.Equal(AppointmentStatuses.Scheduled, appointment.Status);
        unitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task RescheduleAppointmentAsync_WhenDoctorIsInactive_ThrowsInactiveDoctor()
    {
        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            DoctorId = Guid.NewGuid(),
            PatientId = Guid.NewGuid(),
            AppointmentDate = new DateTime(2026, 7, 1, 10, 0, 0, DateTimeKind.Utc),
            DurationMinutes = 30,
            Status = AppointmentStatuses.Scheduled
        };
        var dto = new AppointmentRescheduleDto
        {
            AppointmentDate = new DateTime(2026, 7, 1, 11, 0, 0, DateTimeKind.Utc),
            DurationMinutes = 30
        };

        appointmentRepository
            .Setup(x => x.GetAppointmentAsync(appointment.Id, true))
            .ReturnsAsync(appointment);
        doctorRepository
            .Setup(x => x.GetDoctorAsync(appointment.DoctorId, false))
            .ReturnsAsync(new Doctor { Id = appointment.DoctorId, IsActive = false });

        var service = new AppointmentService(unitOfWork.Object, mapper);

        await Assert.ThrowsAsync<DoctorInactiveException>(() => service.RescheduleAppointmentAsync(appointment.Id, dto));
        appointmentRepository.Verify(x => x.HasOverlappingAppointmentAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<Guid?>()), Times.Never);
    }
}
