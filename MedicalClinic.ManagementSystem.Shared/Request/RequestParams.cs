using System.ComponentModel.DataAnnotations;

namespace MedicalClinic.ManagementSystem.Shared.Request;

public abstract class RequestParams
{

    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;

    [Range(2, 20)]
    public int PageSize { get; set; } = 2;

}

public class PatientRequestParams : RequestParams
{
    [MaxLength(100)]
    public string? SearchTerm { get; set; }

    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(30)]
    public string? PhoneNumber { get; set; }
}

public class DoctorRequestParams : RequestParams
{
    [MaxLength(100)]
    public string? SearchTerm { get; set; }

    [MaxLength(100)]
    public string? Specialty { get; set; }

    public bool? IsActive { get; set; }
}

public class AppointmentRequestParams : RequestParams
{
    public Guid? PatientId { get; set; }
    public Guid? DoctorId { get; set; }
    public DateTime? FromUtc { get; set; }
    public DateTime? ToUtc { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }
}

public class MedicalRecordRequestParams : RequestParams
{
    public Guid? PatientId { get; set; }

    [MaxLength(100)]
    public string? SearchTerm { get; set; }
}

public class PrescriptionRequestParams : RequestParams
{
    public Guid? PatientId { get; set; }
    public Guid? DoctorId { get; set; }
    public Guid? MedicalRecordId { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }
}
