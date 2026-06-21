using MedicalClinic.ManagementSystem.Shared.DTOs.AuthDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MedicalClinic.ManagementSystem.Service.Contracts;

namespace MedicalClinic.ManagementSystem.Presentation.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IServiceManager serviceManager;

    public AuthController(IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser(UserRegistrationDto userRegistrationDto)
    {
        IdentityResult result = await serviceManager.AuthService.RegisterUserAsync(userRegistrationDto);
        return result.Succeeded ? StatusCode(StatusCodes.Status201Created) : BadRequest(result.Errors);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Authenticate(UserAuthDto user)
    {
        if (!await serviceManager.AuthService.ValidateUserAsync(user))
            return Unauthorized();

        var tokenDto = await serviceManager.AuthService.CreateTokenAsync(addTime: true);
        return Ok(tokenDto);
    }
}
