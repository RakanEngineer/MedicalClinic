namespace MedicalClinic.ManagementSystem.Domain.Models.Exceptions;

public sealed class PrescriptionCancelledException : BadRequestException
{
    public PrescriptionCancelledException(Guid prescriptionId)
        : base($"Prescription '{prescriptionId}' is cancelled and cannot be updated.", "Cancelled Prescription")
    {
    }
}
