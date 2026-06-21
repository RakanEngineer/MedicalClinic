using AutoMapper;
using MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;
using MedicalClinic.ManagementSystem.Domain.Models.Entities;
using MedicalClinic.ManagementSystem.Domain.Models.Exceptions;
using MedicalClinic.ManagementSystem.Service.Contracts;
using MedicalClinic.ManagementSystem.Shared.DTOs.MedicalRecords;
using MedicalClinic.ManagementSystem.Shared.Request;

namespace MedicalClinic.ManagementSystem.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public MedicalRecordService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<(IEnumerable<MedicalRecordDto> Items, MetaData MetaData)> GetMedicalRecordsAsync(MedicalRecordRequestParams requestParams, bool trackChanges = false)
    {
        var pagedList = await unitOfWork.MedicalRecordRepository.GetMedicalRecordsAsync(requestParams, trackChanges);
        return (mapper.Map<IEnumerable<MedicalRecordDto>>(pagedList.Items), pagedList.MetaData);
    }

    public async Task<MedicalRecordDto> GetMedicalRecordAsync(Guid id, bool trackChanges = false)
    {
        var record = await unitOfWork.MedicalRecordRepository.GetMedicalRecordAsync(id, trackChanges);
        if (record is null) throw new EntityNotFoundException(nameof(MedicalRecord), id);
        return mapper.Map<MedicalRecordDto>(record);
    }

    public async Task<MedicalRecordDto> CreateMedicalRecordAsync(MedicalRecordCreateDto dto)
    {
        var record = mapper.Map<MedicalRecord>(dto);
        unitOfWork.MedicalRecordRepository.Create(record);
        await unitOfWork.CompleteAsync();
        return mapper.Map<MedicalRecordDto>(record);
    }

    public async Task UpdateMedicalRecordAsync(Guid id, MedicalRecordUpdateDto dto)
    {
        if (id != dto.Id) throw new EntityIdMismatchException(nameof(MedicalRecord), id, dto.Id);
        var record = await unitOfWork.MedicalRecordRepository.GetMedicalRecordAsync(id, trackChanges: true);
        if (record is null) throw new EntityNotFoundException(nameof(MedicalRecord), id);
        mapper.Map(dto, record);
        await unitOfWork.CompleteAsync();
    }

    public async Task DeleteMedicalRecordAsync(Guid id)
    {
        var record = await unitOfWork.MedicalRecordRepository.GetMedicalRecordAsync(id);
        if (record is null) throw new EntityNotFoundException(nameof(MedicalRecord), id);
        unitOfWork.MedicalRecordRepository.Delete(record);
        await unitOfWork.CompleteAsync();
    }
}
