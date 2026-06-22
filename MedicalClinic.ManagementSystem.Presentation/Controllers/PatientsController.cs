using System.Text.Json;
using MedicalClinic.ManagementSystem.Service.Contracts;
using MedicalClinic.ManagementSystem.Shared.Authorization;
using MedicalClinic.ManagementSystem.Shared.DTOs.Patients;
using MedicalClinic.ManagementSystem.Shared.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinic.ManagementSystem.Presentation.Controllers;

[Route("api/v1/patients")]
[ApiController]
[Produces("application/json")]
public class PatientsController : ControllerBase
{
    private readonly IServiceManager serviceManager;

    public PatientsController(IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager;
    }

    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.ClinicRead)]
    [ProducesResponseType(typeof(IEnumerable<PatientDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients([FromQuery] PatientRequestParams requestParams)
    {
        var pagedResult = await serviceManager.PatientService.GetPatientsAsync(requestParams);
        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.MetaData);
        return Ok(pagedResult.Items);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.ClinicRead)]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatientDto>> GetPatient(Guid id) =>
        Ok(await serviceManager.PatientService.GetPatientAsync(id));

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.CanWrite)]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PatientDto>> CreatePatient(PatientCreateDto dto)
    {
        var createdDto = await serviceManager.PatientService.CreatePatientAsync(dto);
        return CreatedAtAction(nameof(GetPatient), new { id = createdDto.Id }, createdDto);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.CanWrite)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePatient(Guid id, PatientUpdateDto dto)
    {
        await serviceManager.PatientService.UpdatePatientAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePatient(Guid id)
    {
        await serviceManager.PatientService.DeletePatientAsync(id);
        return NoContent();
    }
}
