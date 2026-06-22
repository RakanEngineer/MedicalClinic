using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;
using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Infrastructure.Data;
using MedicalClinic.ManagementSystem.Shared.Request;
using Microsoft.EntityFrameworkCore;

namespace MedicalClinic.ManagementSystem.Infrastructure.Repositories;

public class DoctorRepository : RepositoryBase<Doctor>, IDoctorRepository
{
    public DoctorRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Doctor?> GetDoctorAsync(Guid id, bool trackChanges = false) =>
        await FindByCondition(doctor => doctor.Id == id, trackChanges).FirstOrDefaultAsync();

    public async Task<PagedList<Doctor>> GetDoctorsAsync(DoctorRequestParams requestParams, bool trackChanges = false)
    {
        IQueryable<Doctor> doctors = FindAll(trackChanges).OrderBy(doctor => doctor.LastName).ThenBy(doctor => doctor.FirstName);

        if (requestParams.IsActive.HasValue)
            doctors = doctors.Where(doctor => doctor.IsActive == requestParams.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(requestParams.Specialty))
        {
            string specialty = requestParams.Specialty.Trim();
            doctors = doctors.Where(doctor => doctor.Specialty.Contains(specialty));
        }

        if (!string.IsNullOrWhiteSpace(requestParams.SearchTerm))
        {
            string searchTerm = requestParams.SearchTerm.Trim();
            doctors = doctors.Where(doctor =>
                doctor.FirstName.Contains(searchTerm) ||
                doctor.LastName.Contains(searchTerm) ||
                doctor.Specialty.Contains(searchTerm) ||
                doctor.PhoneNumber.Contains(searchTerm) ||
                (doctor.Email != null && doctor.Email.Contains(searchTerm)));
        }

        return await doctors.ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);
    }
}
