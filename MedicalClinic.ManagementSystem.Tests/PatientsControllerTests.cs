using MedicalClinic.ManagementSystem.Presentation.Controllers;
using MedicalClinic.ManagementSystem.Service.Contracts;
using MedicalClinic.ManagementSystem.Shared.DTOs.Patients;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MedicalClinic.ManagementSystem.Tests;

public class PatientsControllerTests
{
    private readonly Mock<IServiceManager> serviceManager = new();
    private readonly Mock<IPatientService> patientService = new();

    public PatientsControllerTests()
    {
        serviceManager.SetupGet(x => x.PatientService).Returns(patientService.Object);
    }

    [Fact]
    public async Task GetPatient_ReturnsOkResult()
    {
        var id = Guid.NewGuid();
        var dto = new PatientDto
        {
            Id = id,
            FirstName = "Amal",
            LastName = "Hassan",
            DateOfBirth = new DateTime(1990, 1, 1),
            Gender = "Female",
            PhoneNumber = "+9647700000000"
        };

        patientService.Setup(x => x.GetPatientAsync(id, false)).ReturnsAsync(dto);

        var controller = new PatientsController(serviceManager.Object);
        var result = await controller.GetPatient(id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(dto, okResult.Value);
    }
}
