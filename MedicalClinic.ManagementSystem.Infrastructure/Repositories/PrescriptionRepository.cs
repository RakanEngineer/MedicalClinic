using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;
using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Infrastructure.Data;
using MedicalClinic.ManagementSystem.Shared.Request;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinic.ManagementSystem.Infrastructure.Repositories;

public class PrescriptionRepository : RepositoryBase<Prescription>, IPrescriptionRepository
{
    public PrescriptionRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Prescription?> GetPrescriptionAsync(Guid id, bool trackChanges = false) =>
        await FindByCondition(prescription => prescription.Id == id, trackChanges).FirstOrDefaultAsync();

    public async Task<PagedList<Prescription>> GetPrescriptionsAsync(PrescriptionRequestParams requestParams, bool trackChanges = false)
    {
        IQueryable<Prescription> prescriptions = FindAll(trackChanges).OrderByDescending(prescription => prescription.IssuedAt);

        if (requestParams.PatientId.HasValue)
            prescriptions = prescriptions.Where(prescription => prescription.PatientId == requestParams.PatientId.Value);

        if (requestParams.DoctorId.HasValue)
            prescriptions = prescriptions.Where(prescription => prescription.DoctorId == requestParams.DoctorId.Value);

        if (requestParams.MedicalRecordId.HasValue)
            prescriptions = prescriptions.Where(prescription => prescription.MedicalRecordId == requestParams.MedicalRecordId.Value);

        if (!string.IsNullOrWhiteSpace(requestParams.Status))
        {
            string status = requestParams.Status.Trim();
            prescriptions = prescriptions.Where(prescription => prescription.Status == status);
        }

        return await prescriptions.ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);
    }
}
