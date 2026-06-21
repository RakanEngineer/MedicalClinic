using MedicalClinic.ManagementSystem.Shared.DTOs.MedicalRecords;
using MedicalClinic.ManagementSystem.Shared.Request;

namespace MedicalClinic.ManagementSystem.Service.Contracts;

public interface IMedicalRecordService
{
    Task<(IEnumerable<MedicalRecordDto> Items, MetaData MetaData)> GetMedicalRecordsAsync(MedicalRecordRequestParams requestParams, bool trackChanges = false);
    Task<MedicalRecordDto> GetMedicalRecordAsync(Guid id, bool trackChanges = false);
    Task<MedicalRecordDto> CreateMedicalRecordAsync(MedicalRecordCreateDto dto);
    Task UpdateMedicalRecordAsync(Guid id, MedicalRecordUpdateDto dto);
    Task DeleteMedicalRecordAsync(Guid id);
}
