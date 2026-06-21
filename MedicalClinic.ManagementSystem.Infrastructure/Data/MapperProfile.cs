using AutoMapper;
using MedicalClinic.ManagementSystem.Shared.DTOs.AuthDtos;
using MedicalClinic.ManagementSystem.Shared.DTOs.Appointments;
using MedicalClinic.ManagementSystem.Shared.DTOs.Doctors;
using MedicalClinic.ManagementSystem.Shared.DTOs.MedicalRecords;
using MedicalClinic.ManagementSystem.Shared.DTOs.Patients;
using MedicalClinic.ManagementSystem.Domain.Models.Entities;

namespace MedicalClinic.ManagementSystem.Infrastructure.Data;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Patient, PatientDto>();
        CreateMap<PatientCreateDto, Patient>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<PatientUpdateDto, Patient>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<Doctor, DoctorDto>();
        CreateMap<DoctorCreateDto, Doctor>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<DoctorUpdateDto, Doctor>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<Appointment, AppointmentDto>();
        CreateMap<AppointmentCreateDto, Appointment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<AppointmentUpdateDto, Appointment>();

        CreateMap<MedicalRecord, MedicalRecordDto>();
        CreateMap<MedicalRecordCreateDto, MedicalRecord>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<MedicalRecordUpdateDto, MedicalRecord>();

        CreateMap<UserRegistrationDto, ApplicationUser>();
    }
}
