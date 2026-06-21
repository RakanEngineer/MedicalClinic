using System.Text.Json;
using MedicalClinic.ManagementSystem.Service.Contracts;
using MedicalClinic.ManagementSystem.Shared.Authorization;
using MedicalClinic.ManagementSystem.Shared.DTOs.MedicalRecords;
using MedicalClinic.ManagementSystem.Shared.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinic.ManagementSystem.Presentation.Controllers;

[Route("api/v1/medical-records")]
[ApiController]
[Produces("application/json")]
public class MedicalRecordsController : ControllerBase
{
    private readonly IServiceManager serviceManager;

    public MedicalRecordsController(IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MedicalRecordDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> GetMedicalRecords([FromQuery] MedicalRecordRequestParams requestParams)
    {
        var pagedResult = await serviceManager.MedicalRecordService.GetMedicalRecordsAsync(requestParams);
        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.MetaData);
        return Ok(pagedResult.Items);
    }

    [HttpGet("patient/{patientId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<MedicalRecordDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MedicalRecordDto>>> GetRecordsByPatient(Guid patientId, [FromQuery] MedicalRecordRequestParams requestParams)
    {
        requestParams.PatientId = patientId;
        var pagedResult = await serviceManager.MedicalRecordService.GetMedicalRecordsAsync(requestParams);
        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.MetaData);
        return Ok(pagedResult.Items);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MedicalRecordDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MedicalRecordDto>> GetMedicalRecord(Guid id) =>
        Ok(await serviceManager.MedicalRecordService.GetMedicalRecordAsync(id));

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.CanWrite)]
    [ProducesResponseType(typeof(MedicalRecordDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MedicalRecordDto>> CreateMedicalRecord(MedicalRecordCreateDto dto)
    {
        var createdDto = await serviceManager.MedicalRecordService.CreateMedicalRecordAsync(dto);
        return CreatedAtAction(nameof(GetMedicalRecord), new { id = createdDto.Id }, createdDto);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.CanWrite)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMedicalRecord(Guid id, MedicalRecordUpdateDto dto)
    {
        await serviceManager.MedicalRecordService.UpdateMedicalRecordAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.CanWrite)]
    public async Task<IActionResult> DeleteMedicalRecord(Guid id)
    {
        await serviceManager.MedicalRecordService.DeleteMedicalRecordAsync(id);
        return NoContent();
    }
}
