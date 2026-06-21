using AutoMapper;
using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;
using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Domain.Models.Exceptions;
using MedicalClinic.ManagementSystem.Services;
using MedicalClinic.ManagementSystem.Shared.DTOs.Patients;
using MedicalClinic.ManagementSystem.Tests.Helpers;
using Moq;

namespace MedicalClinic.ManagementSystem.Tests;

public class PatientServiceTests
{
    private readonly IMapper mapper = MapperFactory.Create();
    private readonly Mock<IUnitOfWork> unitOfWork = new();
    private readonly Mock<IPatientRepository> patientRepository = new();

    public PatientServiceTests()
    {
        unitOfWork.SetupGet(x => x.PatientRepository).Returns(patientRepository.Object);
    }

    [Fact]
    public async Task GetPatientAsync_WhenPatientIsMissing_ThrowsNotFound()
    {
        var id = Guid.NewGuid();
        var service = new PatientService(unitOfWork.Object, mapper);

        patientRepository
            .Setup(x => x.GetPatientAsync(id, false))
            .ReturnsAsync((Patient?)null);

        await Assert.ThrowsAsync<EntityNotFoundException>(() => service.GetPatientAsync(id));
    }

    [Fact]
    public async Task UpdatePatientAsync_WhenIdsDiffer_ThrowsBadRequest()
    {
        var service = new PatientService(unitOfWork.Object, mapper);
        var dto = new PatientUpdateDto
        {
            Id = Guid.NewGuid(),
            FirstName = "Amal",
            LastName = "Hassan",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = "Female",
            PhoneNumber = "+9647700000000"
        };

        await Assert.ThrowsAsync<EntityIdMismatchException>(() => service.UpdatePatientAsync(Guid.NewGuid(), dto));
    }
}
