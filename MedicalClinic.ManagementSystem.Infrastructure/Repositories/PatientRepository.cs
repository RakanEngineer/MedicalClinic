using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;
using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Infrastructure.Data;
using MedicalClinic.ManagementSystem.Shared.Request;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinic.ManagementSystem.Infrastructure.Repositories;

public class PatientRepository : RepositoryBase<Patient>, IPatientRepository
{
    public PatientRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Patient?> GetPatientAsync(Guid id, bool trackChanges = false) =>
        await FindByCondition(patient => patient.Id == id, trackChanges).FirstOrDefaultAsync();

    public async Task<bool> ExistsByPhoneNumberAsync(string phoneNumber, Guid? excludedPatientId = null)
    {
        IQueryable<Patient> patients = FindByCondition(patient => patient.PhoneNumber == phoneNumber.Trim());

        if (excludedPatientId.HasValue)
            patients = patients.Where(patient => patient.Id != excludedPatientId.Value);

        return await patients.AnyAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email, Guid? excludedPatientId = null)
    {
        string normalizedEmail = email.Trim();
        IQueryable<Patient> patients = FindByCondition(patient => patient.Email != null && patient.Email == normalizedEmail);

        if (excludedPatientId.HasValue)
            patients = patients.Where(patient => patient.Id != excludedPatientId.Value);

        return await patients.AnyAsync();
    }

    public async Task<PagedList<Patient>> GetPatientsAsync(PatientRequestParams requestParams, bool trackChanges = false)
    {
        IQueryable<Patient> patients = FindAll(trackChanges).OrderBy(patient => patient.LastName).ThenBy(patient => patient.FirstName);

        if (!string.IsNullOrWhiteSpace(requestParams.Name))
        {
            string name = requestParams.Name.Trim();
            patients = patients.Where(patient =>
                patient.FirstName.Contains(name) ||
                patient.LastName.Contains(name) ||
                (patient.FirstName + " " + patient.LastName).Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(requestParams.PhoneNumber))
        {
            string phoneNumber = requestParams.PhoneNumber.Trim();
            patients = patients.Where(patient => patient.PhoneNumber.Contains(phoneNumber));
        }

        if (!string.IsNullOrWhiteSpace(requestParams.SearchTerm))
        {
            string searchTerm = requestParams.SearchTerm.Trim();
            patients = patients.Where(patient =>
                patient.FirstName.Contains(searchTerm) ||
                patient.LastName.Contains(searchTerm) ||
                patient.PhoneNumber.Contains(searchTerm) ||
                (patient.Email != null && patient.Email.Contains(searchTerm)));
        }

        return await patients.ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);
    }
}
