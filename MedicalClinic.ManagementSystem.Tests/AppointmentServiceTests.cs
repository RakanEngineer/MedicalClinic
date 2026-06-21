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

    public AppointmentServiceTests()
    {
        unitOfWork.SetupGet(x => x.AppointmentRepository).Returns(appointmentRepository.Object);
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
            Status = "Scheduled"
        };

        appointmentRepository
            .Setup(x => x.HasOverlappingAppointmentAsync(doctorId, appointmentDate, null))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<AppointmentOverlapException>(() => service.CreateAppointmentAsync(dto));
        appointmentRepository.Verify(x => x.Create(It.IsAny<Appointment>()), Times.Never);
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
            Status = "Scheduled"
        };

        appointmentRepository
            .Setup(x => x.GetAppointmentAsync(appointment.Id, true))
            .ReturnsAsync(appointment);

        var service = new AppointmentService(unitOfWork.Object, mapper);

        await service.CancelAppointmentAsync(appointment.Id);

        Assert.Equal("Cancelled", appointment.Status);
        unitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
    }
}
