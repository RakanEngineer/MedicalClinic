using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;
using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Infrastructure.Data;
using MedicalClinic.ManagementSystem.Shared.Request;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinic.ManagementSystem.Infrastructure.Repositories;

public class MedicalRecordRepository : RepositoryBase<MedicalRecord>, IMedicalRecordRepository
{
    public MedicalRecordRepository(ApplicationDbContext context) : base(context) { }

    public async Task<MedicalRecord?> GetMedicalRecordAsync(Guid id, bool trackChanges = false) =>
        await FindByCondition(record => record.Id == id, trackChanges).FirstOrDefaultAsync();

    public async Task<PagedList<MedicalRecord>> GetMedicalRecordsAsync(MedicalRecordRequestParams requestParams, bool trackChanges = false)
    {
        IQueryable<MedicalRecord> records = FindAll(trackChanges).OrderByDescending(record => record.RecordDate);

        if (requestParams.PatientId.HasValue)
            records = records.Where(record => record.PatientId == requestParams.PatientId.Value);

        if (!string.IsNullOrWhiteSpace(requestParams.SearchTerm))
        {
            string searchTerm = requestParams.SearchTerm.Trim();
            records = records.Where(record =>
                record.Diagnosis.Contains(searchTerm) ||
                record.Treatment.Contains(searchTerm) ||
                (record.Prescription != null && record.Prescription.Contains(searchTerm)));
        }

        return await records.ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);
    }
}
