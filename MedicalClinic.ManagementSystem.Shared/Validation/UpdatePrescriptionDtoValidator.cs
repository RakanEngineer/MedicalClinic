using FluentValidation;
using MedicalClinic.ManagementSystem.Shared.DTOs.Prescriptions;

namespace MedicalClinic.ManagementSystem.Shared.Validation;

public sealed class UpdatePrescriptionDtoValidator : PrescriptionManipulationDtoValidator<UpdatePrescriptionDto>
{
    public UpdatePrescriptionDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
    }
}
