using System.Text.Json;
using MedicalClinic.ManagementSystem.Service.Contracts;
using MedicalClinic.ManagementSystem.Shared.Authorization;
using MedicalClinic.ManagementSystem.Shared.DTOs.Doctors;
using MedicalClinic.ManagementSystem.Shared.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalClinic.ManagementSystem.Presentation.Controllers;

[Route("api/v1/doctors")]
[ApiController]
[Produces("application/json")]
public class DoctorsController : ControllerBase
{
    private readonly IServiceManager serviceManager;

    public DoctorsController(IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DoctorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors([FromQuery] DoctorRequestParams requestParams)
    {
        var pagedResult = await serviceManager.DoctorService.GetDoctorsAsync(requestParams);
        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.MetaData);
        return Ok(pagedResult.Items);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DoctorDto>> GetDoctor(Guid id) =>
        Ok(await serviceManager.DoctorService.GetDoctorAsync(id));

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.CanWrite)]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DoctorDto>> CreateDoctor(DoctorCreateDto dto)
    {
        var createdDto = await serviceManager.DoctorService.CreateDoctorAsync(dto);
        return CreatedAtAction(nameof(GetDoctor), new { id = createdDto.Id }, createdDto);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.CanWrite)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDoctor(Guid id, DoctorUpdateDto dto)
    {
        await serviceManager.DoctorService.UpdateDoctorAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.CanWrite)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDoctor(Guid id)
    {
        await serviceManager.DoctorService.DeleteDoctorAsync(id);
        return NoContent();
    }
}
