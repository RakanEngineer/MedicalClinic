using AutoMapper;
using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;
using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Domain.Models.Exceptions;
using MedicalClinic.ManagementSystem.Service.Contracts;
using MedicalClinic.ManagementSystem.Shared.DTOs.Doctors;
using MedicalClinic.ManagementSystem.Shared.Request;

namespace MedicalClinic.ManagementSystem.Services;

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<(IEnumerable<DoctorDto> Items, MetaData MetaData)> GetDoctorsAsync(DoctorRequestParams requestParams, bool trackChanges = false)
    {
        var pagedList = await unitOfWork.DoctorRepository.GetDoctorsAsync(requestParams, trackChanges);
        return (mapper.Map<IEnumerable<DoctorDto>>(pagedList.Items), pagedList.MetaData);
    }

    public async Task<DoctorDto> GetDoctorAsync(Guid id, bool trackChanges = false)
    {
        var doctor = await unitOfWork.DoctorRepository.GetDoctorAsync(id, trackChanges);
        if (doctor is null) throw new EntityNotFoundException(nameof(Doctor), id);
        return mapper.Map<DoctorDto>(doctor);
    }

    public async Task<DoctorDto> CreateDoctorAsync(DoctorCreateDto dto)
    {
        var doctor = mapper.Map<Doctor>(dto);
        unitOfWork.DoctorRepository.Create(doctor);
        await unitOfWork.CompleteAsync();
        return mapper.Map<DoctorDto>(doctor);
    }

    public async Task UpdateDoctorAsync(Guid id, DoctorUpdateDto dto)
    {
        if (id != dto.Id) throw new EntityIdMismatchException(nameof(Doctor), id, dto.Id);
        var doctor = await unitOfWork.DoctorRepository.GetDoctorAsync(id, trackChanges: true);
        if (doctor is null) throw new EntityNotFoundException(nameof(Doctor), id);
        mapper.Map(dto, doctor);
        await unitOfWork.CompleteAsync();
    }

    public async Task DeleteDoctorAsync(Guid id)
    {
        var doctor = await unitOfWork.DoctorRepository.GetDoctorAsync(id);
        if (doctor is null) throw new EntityNotFoundException(nameof(Doctor), id);
        unitOfWork.DoctorRepository.Delete(doctor);
        await unitOfWork.CompleteAsync();
    }
}
