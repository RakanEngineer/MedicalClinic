using MedicalClinic.ManagementSystem.Presentation.Controllers;
using MedicalClinic.ManagementSystem.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace MedicalClinic.ManagementSystem.Tests;

public class AuthorizationPolicyTests
{
    [Fact]
    public void PatientReadEndpoints_RequireClinicReadPolicy()
    {
        var method = typeof(PatientsController).GetMethod(nameof(PatientsController.GetPatients));

        var attribute = Assert.Single(method!.GetCustomAttributes(typeof(AuthorizeAttribute), inherit: false).Cast<AuthorizeAttribute>());
        Assert.Equal(AuthorizationPolicies.ClinicRead, attribute.Policy);
    }

    [Fact]
    public void MedicalRecordCreateEndpoint_RequiresClinicalWritePolicy()
    {
        var method = typeof(MedicalRecordsController).GetMethod(nameof(MedicalRecordsController.CreateMedicalRecord));

        var attribute = Assert.Single(method!.GetCustomAttributes(typeof(AuthorizeAttribute), inherit: false).Cast<AuthorizeAttribute>());
        Assert.Equal(AuthorizationPolicies.ClinicalWrite, attribute.Policy);
    }
}
