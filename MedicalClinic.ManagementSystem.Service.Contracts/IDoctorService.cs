using MedicalClinic.ManagementSystem.Shared.DTOs.Doctors;
using MedicalClinic.ManagementSystem.Shared.Request;

namespace MedicalClinic.ManagementSystem.Service.Contracts;

public interface IDoctorService
{
    Task<(IEnumerable<DoctorDto> Items, MetaData MetaData)> GetDoctorsAsync(DoctorRequestParams requestParams, bool trackChanges = false);
    Task<DoctorDto> GetDoctorAsync(Guid id, bool trackChanges = false);
    Task<DoctorDto> CreateDoctorAsync(DoctorCreateDto dto);
    Task UpdateDoctorAsync(Guid id, DoctorUpdateDto dto);
    Task DeleteDoctorAsync(Guid id);
}
