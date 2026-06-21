using FluentValidation;
using MedicalClinic.ManagementSystem.Shared.DTOs.Appointments;

namespace MedicalClinic.ManagementSystem.Shared.Validation;

public sealed class AppointmentUpdateDtoValidator : AppointmentManipulationDtoValidator<AppointmentUpdateDto>
{
    public AppointmentUpdateDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
    }
}
