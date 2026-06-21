using FluentValidation;
using MedicalClinic.ManagementSystem.Shared.DTOs.Patients;

namespace MedicalClinic.ManagementSystem.Shared.Validation;

public sealed class PatientUpdateDtoValidator : PatientManipulationDtoValidator<PatientUpdateDto>
{
    public PatientUpdateDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
    }
}
