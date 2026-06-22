using AutoMapper;
using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;
using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Domain.Models.Exceptions;
using MedicalClinic.ManagementSystem.Service.Contracts;
using MedicalClinic.ManagementSystem.Shared.DTOs.Prescriptions;
using MedicalClinic.ManagementSystem.Shared.Request;

namespace MedicalClinic.ManagementSystem.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public PrescriptionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<(IEnumerable<PrescriptionDto> Items, MetaData MetaData)> GetPrescriptionsAsync(PrescriptionRequestParams requestParams, bool trackChanges = false)
    {
        var pagedList = await unitOfWork.PrescriptionRepository.GetPrescriptionsAsync(requestParams, trackChanges);
        return (mapper.Map<IEnumerable<PrescriptionDto>>(pagedList.Items), pagedList.MetaData);
    }

    public async Task<PrescriptionDto> GetPrescriptionAsync(Guid id, bool trackChanges = false)
    {
        var prescription = await unitOfWork.PrescriptionRepository.GetPrescriptionAsync(id, trackChanges);
        if (prescription is null) throw new EntityNotFoundException(nameof(Prescription), id);
        return mapper.Map<PrescriptionDto>(prescription);
    }

    public async Task<PrescriptionDto> CreatePrescriptionAsync(CreatePrescriptionDto dto)
    {
        var status = NormalizeStatus(dto.Status);
        await EnsurePatientExistsAsync(dto.PatientId);
        await EnsureDoctorIsActiveAsync(dto.DoctorId);
        await EnsureMedicalRecordExistsAsync(dto.MedicalRecordId);

        var prescription = mapper.Map<Prescription>(dto);
        prescription.IssuedAt = dto.IssuedAt ?? DateTime.UtcNow;
        prescription.Status = status;

        unitOfWork.PrescriptionRepository.Create(prescription);
        await unitOfWork.CompleteAsync();
        return mapper.Map<PrescriptionDto>(prescription);
    }

    public async Task UpdatePrescriptionAsync(Guid id, UpdatePrescriptionDto dto)
    {
        if (id != dto.Id) throw new EntityIdMismatchException(nameof(Prescription), id, dto.Id);
        var status = NormalizeStatus(dto.Status);

        var prescription = await unitOfWork.PrescriptionRepository.GetPrescriptionAsync(id, trackChanges: true);
        if (prescription is null) throw new EntityNotFoundException(nameof(Prescription), id);
        if (prescription.Status == PrescriptionStatuses.Cancelled) throw new PrescriptionCancelledException(id);

        await EnsurePatientExistsAsync(dto.PatientId);
        await EnsureDoctorIsActiveAsync(dto.DoctorId);
        await EnsureMedicalRecordExistsAsync(dto.MedicalRecordId);

        mapper.Map(dto, prescription);
        prescription.IssuedAt = dto.IssuedAt ?? prescription.IssuedAt;
        prescription.Status = status;

        await unitOfWork.CompleteAsync();
    }

    public async Task CancelPrescriptionAsync(Guid id)
    {
        var prescription = await unitOfWork.PrescriptionRepository.GetPrescriptionAsync(id, trackChanges: true);
        if (prescription is null) throw new EntityNotFoundException(nameof(Prescription), id);

        prescription.Status = PrescriptionStatuses.Cancelled;
        await unitOfWork.CompleteAsync();
    }

    public async Task DeletePrescriptionAsync(Guid id)
    {
        var prescription = await unitOfWork.PrescriptionRepository.GetPrescriptionAsync(id);
        if (prescription is null) throw new EntityNotFoundException(nameof(Prescription), id);

        unitOfWork.PrescriptionRepository.Delete(prescription);
        await unitOfWork.CompleteAsync();
    }

    private static string NormalizeStatus(string status)
    {
        var normalizedStatus = PrescriptionStatuses.All.FirstOrDefault(allowed =>
            string.Equals(allowed, status?.Trim(), StringComparison.OrdinalIgnoreCase));

        if (normalizedStatus is null)
        {
            throw new PrescriptionStatusException(status);
        }

        return normalizedStatus;
    }

    private async Task EnsurePatientExistsAsync(Guid patientId)
    {
        var patient = await unitOfWork.PatientRepository.GetPatientAsync(patientId);
        if (patient is null) throw new EntityNotFoundException(nameof(Patient), patientId);
    }

    private async Task EnsureDoctorIsActiveAsync(Guid doctorId)
    {
        var doctor = await unitOfWork.DoctorRepository.GetDoctorAsync(doctorId);
        if (doctor is null) throw new EntityNotFoundException(nameof(Doctor), doctorId);
        if (!doctor.IsActive) throw new DoctorInactiveException(doctorId);
    }

    private async Task EnsureMedicalRecordExistsAsync(Guid? medicalRecordId)
    {
        if (!medicalRecordId.HasValue) return;

        var record = await unitOfWork.MedicalRecordRepository.GetMedicalRecordAsync(medicalRecordId.Value);
        if (record is null) throw new EntityNotFoundException(nameof(MedicalRecord), medicalRecordId.Value);
    }
}
