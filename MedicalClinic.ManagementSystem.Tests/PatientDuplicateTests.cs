using AutoMapper;
using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;
using MedicalClinic.ManagementSystem.Domain.Models.Exceptions;
using MedicalClinic.ManagementSystem.Services;
using MedicalClinic.ManagementSystem.Shared.DTOs.Patients;
using MedicalClinic.ManagementSystem.Tests.Helpers;
using Moq;

namespace MedicalClinic.ManagementSystem.Tests;

public class PatientDuplicateTests
{
    private readonly IMapper mapper = MapperFactory.Create();
    private readonly Mock<IUnitOfWork> unitOfWork = new();
    private readonly Mock<IPatientRepository> patientRepository = new();

    public PatientDuplicateTests()
    {
        unitOfWork.SetupGet(x => x.PatientRepository).Returns(patientRepository.Object);
    }

    [Fact]
    public async Task CreatePatientAsync_WhenPhoneNumberExists_ThrowsDuplicate()
    {
        var dto = new PatientCreateDto
        {
            FirstName = "Amal",
            LastName = "Hassan",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = "Female",
            PhoneNumber = " +964 770-000-0000 ",
            Email = "amal@example.com"
        };

        patientRepository
            .Setup(x => x.ExistsByPhoneNumberAsync("+9647700000000", null))
            .ReturnsAsync(true);

        var service = new PatientService(unitOfWork.Object, mapper);

        await Assert.ThrowsAsync<PatientDuplicateException>(() => service.CreatePatientAsync(dto));
        patientRepository.Verify(x => x.Create(It.IsAny<MedicalClinic.ManagementSystem.Domain.Models.Entities.Patient>()), Times.Never);
    }

    [Fact]
    public async Task CreatePatientAsync_NormalizesPhoneAndEmailBeforePersisting()
    {
        var dto = new PatientCreateDto
        {
            FirstName = "Amal",
            LastName = "Hassan",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = "Female",
            PhoneNumber = " +964 770-000-0000 ",
            Email = " AMAL@Example.COM "
        };

        patientRepository
            .Setup(x => x.ExistsByPhoneNumberAsync("+9647700000000", null))
            .ReturnsAsync(false);
        patientRepository
            .Setup(x => x.ExistsByEmailAsync("amal@example.com", null))
            .ReturnsAsync(false);

        var service = new PatientService(unitOfWork.Object, mapper);

        await service.CreatePatientAsync(dto);

        patientRepository.Verify(x => x.Create(It.Is<MedicalClinic.ManagementSystem.Domain.Models.Entities.Patient>(patient =>
            patient.PhoneNumber == "+9647700000000" &&
            patient.Email == "amal@example.com")), Times.Once);
    }
}
