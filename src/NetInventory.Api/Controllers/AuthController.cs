using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetInventory.Api.Common;
using NetInventory.Api.Common.Extensions;
using NetInventory.Api.Requests.Auth;
using NetInventory.Application.Auth.Commands.Login;
using NetInventory.Application.Auth.Commands.Refresh;
using NetInventory.Application.Auth.Commands.Register;
using NetInventory.Application.Common;

namespace NetInventory.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    IDispatcher dispatcher
    ) : ControllerBase
{
    /// <summary>Valida que el token actual sea aceptado por el servidor.</summary>
    [HttpGet("ping")]
    [Authorize]
    public IActionResult Ping() => Ok(ApiResponse.Ok());
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken ct = default)
    {
        var result = await dispatcher.SendAsync(new RegisterCommand(request.Email, request.Password), ct);

        return result.ToActionResult(this, () => StatusCode(StatusCodes.Status201Created, ApiResponse.Ok()));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken ct = default)
    {
        var result = await dispatcher.SendAsync(new LoginCommand(request.Email, request.Password), ct);

        return result.ToActionResult(this, token => Ok(
            ApiResponse<object>.Ok(new
            {
                token.Token,
                token.ExpiresAt,
                token.RefreshToken,
                token.RefreshTokenExpiresAt
            })));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshRequest request,
        CancellationToken ct = default)
    {
        var result = await dispatcher.SendAsync(new RefreshCommand(request.RefreshToken), ct);

        return result.ToActionResult(this, token => Ok(
            ApiResponse<object>.Ok(new
            {
                token.Token,
                token.ExpiresAt,
                token.RefreshToken,
                token.RefreshTokenExpiresAt
            })));
    }
}
