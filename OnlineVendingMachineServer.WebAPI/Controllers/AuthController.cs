using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineVendingMachineServer.WebAPI.DTOs;
using OnlineVendingMachineServer.WebAPI.Services;

namespace OnlineVendingMachineServer.WebAPI.Controllers;

public sealed class AuthController(
    IAuthService authService) : ApiController
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDto request, CancellationToken cancellationToken)
    {
        var response = await authService.Login(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto request, CancellationToken cancellationToken)
    {
        var response = await authService.ChangePassword(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> GoogleLogin(GoogleLoginRequestDto request, CancellationToken cancellationToken)
    {
        var response = await authService.GoogleLogin(request, cancellationToken);
        return StatusCode(response.StatusCode, response);
    }
}
