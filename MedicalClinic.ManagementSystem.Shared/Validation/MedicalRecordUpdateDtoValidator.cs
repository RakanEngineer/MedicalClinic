using FluentValidation;
using MedicalClinic.ManagementSystem.Shared.DTOs.MedicalRecords;

namespace MedicalClinic.ManagementSystem.Shared.Validation;

public sealed class MedicalRecordUpdateDtoValidator : MedicalRecordManipulationDtoValidator<MedicalRecordUpdateDto>
{
    public MedicalRecordUpdateDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
    }
}
