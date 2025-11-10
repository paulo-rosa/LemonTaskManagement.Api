using LemonTaskManagement.Api.Models;
using LemonTaskManagement.Domain.Commands.Commands;
using LemonTaskManagement.Domain.Commands.Interfaces.CommandServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LemonTaskManagement.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationCommandService _authenticationCommandService;

    public AuthenticationController(IAuthenticationCommandService authenticationCommandService)
    {
        _authenticationCommandService = authenticationCommandService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(200, Type = typeof(ApiResponse<LoginResponse>))]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCommand command)
    {
        var response = await _authenticationCommandService.LoginAsync(command);

        if (!response.Success)
        {
            return Unauthorized(new ApiResponse<LoginResponse>(401, response.Message ?? "Authentication failed", response));
        }

        return Ok(new ApiResponse<LoginResponse>(200, "Login successful", response));
    }
}
