using FluentValidation;
using MedicalClinic.ManagementSystem.Shared.DTOs.Patients;

namespace MedicalClinic.ManagementSystem.Shared.Validation;

public abstract class PatientManipulationDtoValidator<T> : AbstractValidator<T>
    where T : PatientManipulationDto
{
    protected PatientManipulationDtoValidator()
    {
        RuleFor(dto => dto.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.LastName).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.DateOfBirth)
            .NotEmpty()
            .LessThan(DateTime.UtcNow.Date);
        RuleFor(dto => dto.Gender).NotEmpty().MaximumLength(30);
        RuleFor(dto => dto.PhoneNumber).NotEmpty().MaximumLength(30);
        RuleFor(dto => dto.Email).EmailAddress().MaximumLength(254).When(dto => !string.IsNullOrWhiteSpace(dto.Email));
        RuleFor(dto => dto.Address).MaximumLength(250);
    }
}
