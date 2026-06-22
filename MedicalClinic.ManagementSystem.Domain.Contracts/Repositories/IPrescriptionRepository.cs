using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Shared.Request;

namespace MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;

public interface IPrescriptionRepository : IRepositoryBase<Prescription>
{
    Task<PagedList<Prescription>> GetPrescriptionsAsync(PrescriptionRequestParams requestParams, bool trackChanges = false);
    Task<Prescription?> GetPrescriptionAsync(Guid id, bool trackChanges = false);
}
