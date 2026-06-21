using FluentValidation;
using MedicalClinic.ManagementSystem.Shared.DTOs.Appointments;

namespace MedicalClinic.ManagementSystem.Shared.Validation;

public abstract class AppointmentManipulationDtoValidator<T> : AbstractValidator<T>
    where T : AppointmentManipulationDto
{
    protected AppointmentManipulationDtoValidator()
    {
        RuleFor(dto => dto.AppointmentDate).NotEmpty();
        RuleFor(dto => dto.Status).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Notes).MaximumLength(500);
        RuleFor(dto => dto.DoctorId).NotEmpty();
        RuleFor(dto => dto.PatientId).NotEmpty();
    }
}
