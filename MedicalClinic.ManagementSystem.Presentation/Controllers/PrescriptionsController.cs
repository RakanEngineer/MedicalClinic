using System.Text.Json;
using MedicalClinic.ManagementSystem.Service.Contracts;
using MedicalClinic.ManagementSystem.Shared.Authorization;
using MedicalClinic.ManagementSystem.Shared.DTOs.Prescriptions;
using MedicalClinic.ManagementSystem.Shared.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinic.ManagementSystem.Presentation.Controllers;

[Route("api/v1/prescriptions")]
[ApiController]
[Produces("application/json")]
public class PrescriptionsController : ControllerBase
{
    private readonly IServiceManager serviceManager;

    public PrescriptionsController(IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager;
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.ClinicRead)]
    [ProducesResponseType(typeof(PrescriptionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PrescriptionDto>> GetPrescription(Guid id) =>
        Ok(await serviceManager.PrescriptionService.GetPrescriptionAsync(id));

    [HttpGet("patient/{patientId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.ClinicRead)]
    [ProducesResponseType(typeof(IEnumerable<PrescriptionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetPrescriptionsByPatient(Guid patientId, [FromQuery] PrescriptionRequestParams requestParams)
    {
        requestParams.PatientId = patientId;
        var pagedResult = await serviceManager.PrescriptionService.GetPrescriptionsAsync(requestParams);
        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.MetaData);
        return Ok(pagedResult.Items);
    }

    [HttpGet("doctor/{doctorId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.ClinicRead)]
    [ProducesResponseType(typeof(IEnumerable<PrescriptionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetPrescriptionsByDoctor(Guid doctorId, [FromQuery] PrescriptionRequestParams requestParams)
    {
        requestParams.DoctorId = doctorId;
        var pagedResult = await serviceManager.PrescriptionService.GetPrescriptionsAsync(requestParams);
        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.MetaData);
        return Ok(pagedResult.Items);
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.ClinicalWrite)]
    [ProducesResponseType(typeof(PrescriptionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PrescriptionDto>> CreatePrescription(CreatePrescriptionDto dto)
    {
        var createdDto = await serviceManager.PrescriptionService.CreatePrescriptionAsync(dto);
        return CreatedAtAction(nameof(GetPrescription), new { id = createdDto.Id }, createdDto);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.ClinicalWrite)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePrescription(Guid id, UpdatePrescriptionDto dto)
    {
        await serviceManager.PrescriptionService.UpdatePrescriptionAsync(id, dto);
        return NoContent();
    }

    [HttpPatch("{id:guid}/cancel")]
    [Authorize(Policy = AuthorizationPolicies.ClinicalWrite)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelPrescription(Guid id)
    {
        await serviceManager.PrescriptionService.CancelPrescriptionAsync(id);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePrescription(Guid id)
    {
        await serviceManager.PrescriptionService.DeletePrescriptionAsync(id);
        return NoContent();
    }
}
