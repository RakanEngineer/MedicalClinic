using AutoMapper;
using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;
using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Domain.Models.Exceptions;
using MedicalClinic.ManagementSystem.Service.Contracts;
using MedicalClinic.ManagementSystem.Shared.DTOs.Patients;
using MedicalClinic.ManagementSystem.Shared.Request;

namespace MedicalClinic.ManagementSystem.Services;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public PatientService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<(IEnumerable<PatientDto> Items, MetaData MetaData)> GetPatientsAsync(PatientRequestParams requestParams, bool trackChanges = false)
    {
        var pagedList = await unitOfWork.PatientRepository.GetPatientsAsync(requestParams, trackChanges);
        return (mapper.Map<IEnumerable<PatientDto>>(pagedList.Items), pagedList.MetaData);
    }

    public async Task<PatientDto> GetPatientAsync(Guid id, bool trackChanges = false)
    {
        var patient = await unitOfWork.PatientRepository.GetPatientAsync(id, trackChanges);
        if (patient is null) throw new EntityNotFoundException(nameof(Patient), id);
        return mapper.Map<PatientDto>(patient);
    }

    public async Task<PatientDto> CreatePatientAsync(PatientCreateDto dto)
    {
        var normalizedPhoneNumber = NormalizePhoneNumber(dto.PhoneNumber);
        var normalizedEmail = NormalizeEmail(dto.Email);

        await EnsurePatientIsUniqueAsync(normalizedPhoneNumber, normalizedEmail);

        var patient = mapper.Map<Patient>(dto);
        patient.PhoneNumber = normalizedPhoneNumber;
        patient.Email = normalizedEmail;
        unitOfWork.PatientRepository.Create(patient);
        await unitOfWork.CompleteAsync();
        return mapper.Map<PatientDto>(patient);
    }

    public async Task UpdatePatientAsync(Guid id, PatientUpdateDto dto)
    {
        if (id != dto.Id) throw new EntityIdMismatchException(nameof(Patient), id, dto.Id);
        var patient = await unitOfWork.PatientRepository.GetPatientAsync(id, trackChanges: true);
        if (patient is null) throw new EntityNotFoundException(nameof(Patient), id);

        var normalizedPhoneNumber = NormalizePhoneNumber(dto.PhoneNumber);
        var normalizedEmail = NormalizeEmail(dto.Email);

        await EnsurePatientIsUniqueAsync(normalizedPhoneNumber, normalizedEmail, id);

        mapper.Map(dto, patient);
        patient.PhoneNumber = normalizedPhoneNumber;
        patient.Email = normalizedEmail;
        await unitOfWork.CompleteAsync();
    }

    public async Task DeletePatientAsync(Guid id)
    {
        var patient = await unitOfWork.PatientRepository.GetPatientAsync(id);
        if (patient is null) throw new EntityNotFoundException(nameof(Patient), id);
        unitOfWork.PatientRepository.Delete(patient);
        await unitOfWork.CompleteAsync();
    }

    private async Task EnsurePatientIsUniqueAsync(string phoneNumber, string? email, Guid? excludedPatientId = null)
    {
        if (await unitOfWork.PatientRepository.ExistsByPhoneNumberAsync(phoneNumber, excludedPatientId))
            throw new PatientDuplicateException("phone number", phoneNumber);

        if (!string.IsNullOrWhiteSpace(email) && await unitOfWork.PatientRepository.ExistsByEmailAsync(email, excludedPatientId))
            throw new PatientDuplicateException("email", email);
    }

    private static string NormalizePhoneNumber(string phoneNumber)
    {
        string trimmed = phoneNumber.Trim();
        return trimmed
            .Replace(" ", string.Empty)
            .Replace("-", string.Empty)
            .Replace("(", string.Empty)
            .Replace(")", string.Empty);
    }

    private static string? NormalizeEmail(string? email) =>
        string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToLowerInvariant();
}
