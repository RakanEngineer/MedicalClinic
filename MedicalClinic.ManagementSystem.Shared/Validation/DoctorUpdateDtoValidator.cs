using FluentValidation;
using MedicalClinic.ManagementSystem.Shared.DTOs.Doctors;

namespace MedicalClinic.ManagementSystem.Shared.Validation;

public sealed class DoctorUpdateDtoValidator : DoctorManipulationDtoValidator<DoctorUpdateDto>
{
    public DoctorUpdateDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
    }
}
