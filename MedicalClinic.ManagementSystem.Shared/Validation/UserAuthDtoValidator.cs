using FluentValidation;
using MedicalClinic.ManagementSystem.Shared.DTOs.AuthDtos;

namespace MedicalClinic.ManagementSystem.Shared.Validation;

public class UserAuthDtoValidator : AbstractValidator<UserAuthDto>
{
    public UserAuthDtoValidator()
    {
        RuleFor(dto => dto.UserName)
            .NotEmpty();

        RuleFor(dto => dto.Password)
            .NotEmpty();
    }
}
