using MedicalClinic.ManagementSystem.Shared.DTOs.AuthDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedicalClinic.ManagementSystem.Service.Contracts;

namespace MedicalClinic.ManagementSystem.Presentation.Controllers;

[Route("api/v1/token")]
[ApiController]
public class TokenController(IAuthService authenticationService) : ControllerBase
{
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenDto>> RefreshToken(TokenDto token) =>
         Ok(await authenticationService.RefreshTokenAsync(token));
}
