using AutoMapper;
using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;
using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Domain.Models.Exceptions;
using MedicalClinic.ManagementSystem.Services;
using MedicalClinic.ManagementSystem.Shared.DTOs.Prescriptions;
using MedicalClinic.ManagementSystem.Tests.Helpers;
using Moq;

namespace MedicalClinic.ManagementSystem.Tests;

public class PrescriptionServiceTests
{
    private readonly IMapper mapper = MapperFactory.Create();
    private readonly Mock<IUnitOfWork> unitOfWork = new();
    private readonly Mock<IPrescriptionRepository> prescriptionRepository = new();
    private readonly Mock<IPatientRepository> patientRepository = new();
    private readonly Mock<IDoctorRepository> doctorRepository = new();
    private readonly Mock<IMedicalRecordRepository> medicalRecordRepository = new();

    public PrescriptionServiceTests()
    {
        unitOfWork.SetupGet(x => x.PrescriptionRepository).Returns(prescriptionRepository.Object);
        unitOfWork.SetupGet(x => x.PatientRepository).Returns(patientRepository.Object);
        unitOfWork.SetupGet(x => x.DoctorRepository).Returns(doctorRepository.Object);
        unitOfWork.SetupGet(x => x.MedicalRecordRepository).Returns(medicalRecordRepository.Object);
    }

    [Fact]
    public async Task CreatePrescriptionAsync_WhenDataIsValid_CreatesPrescription()
    {
        var patientId = Guid.NewGuid();
        var doctorId = Guid.NewGuid();
        var issuedAt = new DateTime(2026, 7, 1, 9, 0, 0, DateTimeKind.Utc);
        var dto = new CreatePrescriptionDto
        {
            PatientId = patientId,
            DoctorId = doctorId,
            MedicationName = "Amoxicillin",
            Dosage = "500mg",
            Frequency = "Twice daily",
            Duration = "7 days",
            IssuedAt = issuedAt,
            Status = PrescriptionStatuses.Active
        };

        patientRepository.Setup(x => x.GetPatientAsync(patientId, false)).ReturnsAsync(new Patient { Id = patientId });
        doctorRepository.Setup(x => x.GetDoctorAsync(doctorId, false)).ReturnsAsync(new Doctor { Id = doctorId, IsActive = true });

        var service = new PrescriptionService(unitOfWork.Object, mapper);

        var result = await service.CreatePrescriptionAsync(dto);

        Assert.Equal(PrescriptionStatuses.Active, result.Status);
        Assert.Equal(issuedAt, result.IssuedAt);
        prescriptionRepository.Verify(x => x.Create(It.Is<Prescription>(prescription =>
            prescription.PatientId == patientId &&
            prescription.DoctorId == doctorId &&
            prescription.Status == PrescriptionStatuses.Active &&
            prescription.IssuedAt == issuedAt)), Times.Once);
        unitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task CreatePrescriptionAsync_WhenDoctorIsInactive_ThrowsInactiveDoctor()
    {
        var patientId = Guid.NewGuid();
        var doctorId = Guid.NewGuid();
        var dto = CreateValidCreateDto(patientId, doctorId);

        patientRepository.Setup(x => x.GetPatientAsync(patientId, false)).ReturnsAsync(new Patient { Id = patientId });
        doctorRepository.Setup(x => x.GetDoctorAsync(doctorId, false)).ReturnsAsync(new Doctor { Id = doctorId, IsActive = false });

        var service = new PrescriptionService(unitOfWork.Object, mapper);

        await Assert.ThrowsAsync<DoctorInactiveException>(() => service.CreatePrescriptionAsync(dto));
        prescriptionRepository.Verify(x => x.Create(It.IsAny<Prescription>()), Times.Never);
    }

    [Fact]
    public async Task CreatePrescriptionAsync_WhenPatientIsMissing_ThrowsNotFound()
    {
        var patientId = Guid.NewGuid();
        var dto = CreateValidCreateDto(patientId, Guid.NewGuid());

        patientRepository.Setup(x => x.GetPatientAsync(patientId, false)).ReturnsAsync((Patient?)null);

        var service = new PrescriptionService(unitOfWork.Object, mapper);

        await Assert.ThrowsAsync<EntityNotFoundException>(() => service.CreatePrescriptionAsync(dto));
        doctorRepository.Verify(x => x.GetDoctorAsync(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async Task CreatePrescriptionAsync_WhenStatusIsInvalid_ThrowsStatusException()
    {
        var dto = new CreatePrescriptionDto
        {
            PatientId = Guid.NewGuid(),
            DoctorId = Guid.NewGuid(),
            MedicationName = "Amoxicillin",
            Dosage = "500mg",
            Frequency = "Twice daily",
            Duration = "7 days",
            Status = "Paused"
        };

        var service = new PrescriptionService(unitOfWork.Object, mapper);

        await Assert.ThrowsAsync<PrescriptionStatusException>(() => service.CreatePrescriptionAsync(dto));
        patientRepository.Verify(x => x.GetPatientAsync(It.IsAny<Guid>(), It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async Task CancelPrescriptionAsync_WhenPrescriptionExists_SetsStatusToCancelled()
    {
        var prescription = new Prescription
        {
            Id = Guid.NewGuid(),
            PatientId = Guid.NewGuid(),
            DoctorId = Guid.NewGuid(),
            MedicationName = "Amoxicillin",
            Dosage = "500mg",
            Frequency = "Twice daily",
            Duration = "7 days",
            Status = PrescriptionStatuses.Active
        };

        prescriptionRepository
            .Setup(x => x.GetPrescriptionAsync(prescription.Id, true))
            .ReturnsAsync(prescription);

        var service = new PrescriptionService(unitOfWork.Object, mapper);

        await service.CancelPrescriptionAsync(prescription.Id);

        Assert.Equal(PrescriptionStatuses.Cancelled, prescription.Status);
        unitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdatePrescriptionAsync_WhenPrescriptionIsCancelled_ThrowsCancelled()
    {
        var prescription = new Prescription
        {
            Id = Guid.NewGuid(),
            PatientId = Guid.NewGuid(),
            DoctorId = Guid.NewGuid(),
            MedicationName = "Amoxicillin",
            Dosage = "500mg",
            Frequency = "Twice daily",
            Duration = "7 days",
            Status = PrescriptionStatuses.Cancelled
        };
        var dto = new UpdatePrescriptionDto
        {
            Id = prescription.Id,
            PatientId = prescription.PatientId,
            DoctorId = prescription.DoctorId,
            MedicationName = "Amoxicillin",
            Dosage = "500mg",
            Frequency = "Twice daily",
            Duration = "10 days",
            Status = PrescriptionStatuses.Active
        };

        prescriptionRepository
            .Setup(x => x.GetPrescriptionAsync(prescription.Id, true))
            .ReturnsAsync(prescription);

        var service = new PrescriptionService(unitOfWork.Object, mapper);

        await Assert.ThrowsAsync<PrescriptionCancelledException>(() => service.UpdatePrescriptionAsync(prescription.Id, dto));
        unitOfWork.Verify(x => x.CompleteAsync(), Times.Never);
    }

    private static CreatePrescriptionDto CreateValidCreateDto(Guid patientId, Guid doctorId) =>
        new()
        {
            PatientId = patientId,
            DoctorId = doctorId,
            MedicationName = "Amoxicillin",
            Dosage = "500mg",
            Frequency = "Twice daily",
            Duration = "7 days",
            Status = PrescriptionStatuses.Active
        };
}
