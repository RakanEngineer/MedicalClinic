using FluentValidation;
using MedicalClinic.ManagementSystem.Shared.DTOs.Appointments;

namespace MedicalClinic.ManagementSystem.Shared.Validation;

public abstract class AppointmentManipulationDtoValidator<T> : AbstractValidator<T>
    where T : AppointmentManipulationDto
{
    private static readonly string[] AllowedStatuses = ["Scheduled", "Completed", "Cancelled", "NoShow"];

    protected AppointmentManipulationDtoValidator()
    {
        RuleFor(dto => dto.AppointmentDate).NotEmpty();
        RuleFor(dto => dto.DurationMinutes).InclusiveBetween(5, 480);
        RuleFor(dto => dto.Status)
            .NotEmpty()
            .Must(status => AllowedStatuses.Contains(status))
            .WithMessage($"Status must be one of: {string.Join(", ", AllowedStatuses)}.");
        RuleFor(dto => dto.Notes).MaximumLength(500);
        RuleFor(dto => dto.DoctorId).NotEmpty();
        RuleFor(dto => dto.PatientId).NotEmpty();
    }
}
