using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Shared.Request;

namespace MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;

public interface IPatientRepository : IRepositoryBase<Patient>
{
    Task<PagedList<Patient>> GetPatientsAsync(PatientRequestParams requestParams, bool trackChanges = false);
    Task<Patient?> GetPatientAsync(Guid id, bool trackChanges = false);
}
