using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Shared.Request;

namespace MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;

public interface IMedicalRecordRepository : IRepositoryBase<MedicalRecord>
{
    Task<PagedList<MedicalRecord>> GetMedicalRecordsAsync(MedicalRecordRequestParams requestParams, bool trackChanges = false);
    Task<MedicalRecord?> GetMedicalRecordAsync(Guid id, bool trackChanges = false);
}
