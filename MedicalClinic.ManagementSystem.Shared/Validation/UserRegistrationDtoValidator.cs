using FluentValidation;
using MedicalClinic.ManagementSystem.Shared.Authorization;
using MedicalClinic.ManagementSystem.Shared.DTOs.AuthDtos;

namespace MedicalClinic.ManagementSystem.Shared.Validation;

public class UserRegistrationDtoValidator : AbstractValidator<UserRegistrationDto>
{
    public UserRegistrationDtoValidator()
    {
        RuleFor(dto => dto.DisplayName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(dto => dto.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(dto => dto.UserName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(dto => dto.Password)
            .NotEmpty()
            .MinimumLength(8);

        RuleFor(dto => dto.Role)
            .Must(role => role is ClinicRoles.Doctor or ClinicRoles.Receptionist or ClinicRoles.Admin)
            .WithMessage("Role must be Doctor, Receptionist, or Admin.");
    }
}
