using FluentValidation;
using MedicalClinic.ManagementSystem.Shared.DTOs.Prescriptions;

namespace MedicalClinic.ManagementSystem.Shared.Validation;

public abstract class PrescriptionManipulationDtoValidator<T> : AbstractValidator<T>
    where T : PrescriptionManipulationDto
{
    private static readonly string[] AllowedStatuses = ["Active", "Completed", "Cancelled"];

    protected PrescriptionManipulationDtoValidator()
    {
        RuleFor(dto => dto.PatientId).NotEmpty();
        RuleFor(dto => dto.DoctorId).NotEmpty();
        RuleFor(dto => dto.MedicationName).NotEmpty().MaximumLength(150);
        RuleFor(dto => dto.Dosage).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.Frequency).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.Duration).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.Instructions).MaximumLength(1000);
        RuleFor(dto => dto.Status)
            .NotEmpty()
            .Must(status => AllowedStatuses.Contains(status))
            .WithMessage($"Status must be one of: {string.Join(", ", AllowedStatuses)}.");
    }
}
