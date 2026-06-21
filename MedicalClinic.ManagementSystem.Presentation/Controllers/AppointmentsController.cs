using System.Text.Json;
using MedicalClinic.ManagementSystem.Service.Contracts;
using MedicalClinic.ManagementSystem.Shared.Authorization;
using MedicalClinic.ManagementSystem.Shared.DTOs.Appointments;
using MedicalClinic.ManagementSystem.Shared.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinic.ManagementSystem.Presentation.Controllers;

[Route("api/v1/appointments")]
[ApiController]
[Produces("application/json")]
public class AppointmentsController : ControllerBase
{
    private readonly IServiceManager serviceManager;

    public AppointmentsController(IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments([FromQuery] AppointmentRequestParams requestParams)
    {
        var pagedResult = await serviceManager.AppointmentService.GetAppointmentsAsync(requestParams);
        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.MetaData);
        return Ok(pagedResult.Items);
    }

    [HttpGet("patient/{patientId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByPatient(Guid patientId, [FromQuery] AppointmentRequestParams requestParams)
    {
        requestParams.PatientId = patientId;
        var pagedResult = await serviceManager.AppointmentService.GetAppointmentsAsync(requestParams);
        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.MetaData);
        return Ok(pagedResult.Items);
    }

    [HttpGet("doctor/{doctorId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointmentsByDoctor(Guid doctorId, [FromQuery] AppointmentRequestParams requestParams)
    {
        requestParams.DoctorId = doctorId;
        var pagedResult = await serviceManager.AppointmentService.GetAppointmentsAsync(requestParams);
        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.MetaData);
        return Ok(pagedResult.Items);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentDto>> GetAppointment(Guid id) =>
        Ok(await serviceManager.AppointmentService.GetAppointmentAsync(id));

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.CanWrite)]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AppointmentDto>> CreateAppointment(AppointmentCreateDto dto)
    {
        var createdDto = await serviceManager.AppointmentService.CreateAppointmentAsync(dto);
        return CreatedAtAction(nameof(GetAppointment), new { id = createdDto.Id }, createdDto);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.CanWrite)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAppointment(Guid id, AppointmentUpdateDto dto)
    {
        await serviceManager.AppointmentService.UpdateAppointmentAsync(id, dto);
        return NoContent();
    }

    [HttpPatch("{id:guid}/cancel")]
    [Authorize(Policy = AuthorizationPolicies.CanWrite)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelAppointment(Guid id)
    {
        await serviceManager.AppointmentService.CancelAppointmentAsync(id);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.CanWrite)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAppointment(Guid id)
    {
        await serviceManager.AppointmentService.DeleteAppointmentAsync(id);
        return NoContent();
    }
}
