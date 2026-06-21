using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Shared.Request;

namespace MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;

public interface IDoctorRepository : IRepositoryBase<Doctor>
{
    Task<PagedList<Doctor>> GetDoctorsAsync(DoctorRequestParams requestParams, bool trackChanges = false);
    Task<Doctor?> GetDoctorAsync(Guid id, bool trackChanges = false);
}
