using MedicalClinic.ManagementSystem.Shared.DTOs.Appointments;
using MedicalClinic.ManagementSystem.Shared.DTOs.Patients;
using MedicalClinic.ManagementSystem.Shared.Validation;

namespace MedicalClinic.ManagementSystem.Tests;

public class ValidatorTests
{
    [Fact]
    public void AppointmentCreateValidator_WhenStatusIsInvalid_Fails()
    {
        var validator = new AppointmentCreateDtoValidator();
        var dto = new AppointmentCreateDto
        {
            AppointmentDate = DateTime.UtcNow.AddDays(1),
            DurationMinutes = 30,
            Status = "Waiting",
            DoctorId = Guid.NewGuid(),
            PatientId = Guid.NewGuid()
        };

        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(AppointmentCreateDto.Status));
    }

    [Fact]
    public void AppointmentRescheduleValidator_WhenDurationIsTooShort_Fails()
    {
        var validator = new AppointmentRescheduleDtoValidator();
        var dto = new AppointmentRescheduleDto
        {
            AppointmentDate = DateTime.UtcNow.AddDays(1),
            DurationMinutes = 1
        };

        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(AppointmentRescheduleDto.DurationMinutes));
    }

    [Fact]
    public void PatientCreateValidator_WhenDateOfBirthIsFuture_Fails()
    {
        var validator = new PatientCreateDtoValidator();
        var dto = new PatientCreateDto
        {
            FirstName = "Amal",
            LastName = "Hassan",
            DateOfBirth = DateTime.UtcNow.AddDays(1),
            Gender = "Female",
            PhoneNumber = "+9647700000000"
        };

        var result = validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(PatientCreateDto.DateOfBirth));
    }
}
