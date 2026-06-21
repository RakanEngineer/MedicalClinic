using FluentValidation;
using MedicalClinic.ManagementSystem.Shared.DTOs.Doctors;

namespace MedicalClinic.ManagementSystem.Shared.Validation;

public abstract class DoctorManipulationDtoValidator<T> : AbstractValidator<T>
    where T : DoctorManipulationDto
{
    protected DoctorManipulationDtoValidator()
    {
        RuleFor(dto => dto.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.LastName).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.Specialty).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.PhoneNumber).NotEmpty().MaximumLength(30);
        RuleFor(dto => dto.Email).EmailAddress().MaximumLength(254).When(dto => !string.IsNullOrWhiteSpace(dto.Email));
    }
}
