using FluentValidation;
using MedicalClinic.ManagementSystem.Shared.DTOs.MedicalRecords;

namespace MedicalClinic.ManagementSystem.Shared.Validation;

public abstract class MedicalRecordManipulationDtoValidator<T> : AbstractValidator<T>
    where T : MedicalRecordManipulationDto
{
    protected MedicalRecordManipulationDtoValidator()
    {
        RuleFor(dto => dto.Diagnosis).NotEmpty().MaximumLength(500);
        RuleFor(dto => dto.Treatment).NotEmpty().MaximumLength(500);
        RuleFor(dto => dto.Prescription).MaximumLength(1000);
        RuleFor(dto => dto.RecordDate).NotEmpty();
        RuleFor(dto => dto.PatientId).NotEmpty();
    }
}
