using MedicalClinic.ManagementSystem.Shared.DTOs.Patients;
using MedicalClinic.ManagementSystem.Shared.Request;

namespace MedicalClinic.ManagementSystem.Service.Contracts;

public interface IPatientService
{
    Task<(IEnumerable<PatientDto> Items, MetaData MetaData)> GetPatientsAsync(PatientRequestParams requestParams, bool trackChanges = false);
    Task<PatientDto> GetPatientAsync(Guid id, bool trackChanges = false);
    Task<PatientDto> CreatePatientAsync(PatientCreateDto dto);
    Task UpdatePatientAsync(Guid id, PatientUpdateDto dto);
    Task DeletePatientAsync(Guid id);
}
