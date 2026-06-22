using FluentValidation;
using MedicalClinic.ManagementSystem.Shared.DTOs.Appointments;

namespace MedicalClinic.ManagementSystem.Shared.Validation;

public sealed class AppointmentRescheduleDtoValidator : AbstractValidator<AppointmentRescheduleDto>
{
    public AppointmentRescheduleDtoValidator()
    {
        RuleFor(dto => dto.AppointmentDate).NotEmpty();
        RuleFor(dto => dto.DurationMinutes).InclusiveBetween(5, 480);
    }
}
