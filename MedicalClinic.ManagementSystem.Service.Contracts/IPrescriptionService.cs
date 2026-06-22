using MedicalClinic.ManagementSystem.Shared.DTOs.Prescriptions;
using MedicalClinic.ManagementSystem.Shared.Request;

namespace MedicalClinic.ManagementSystem.Service.Contracts;

public interface IPrescriptionService
{
    Task<(IEnumerable<PrescriptionDto> Items, MetaData MetaData)> GetPrescriptionsAsync(PrescriptionRequestParams requestParams, bool trackChanges = false);
    Task<PrescriptionDto> GetPrescriptionAsync(Guid id, bool trackChanges = false);
    Task<PrescriptionDto> CreatePrescriptionAsync(CreatePrescriptionDto dto);
    Task UpdatePrescriptionAsync(Guid id, UpdatePrescriptionDto dto);
    Task CancelPrescriptionAsync(Guid id);
    Task DeletePrescriptionAsync(Guid id);
}
