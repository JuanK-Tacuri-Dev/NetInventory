using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetInventory.Api.Requests.Diagnostics;
using NetInventory.Resources;

namespace NetInventory.Api.Controllers;

[ApiController]
[Route("api/diagnostics")]
[Authorize]
public sealed class DiagnosticsController : ControllerBase
{

    [HttpPost("error")]
    public IActionResult SimulateError(
        [FromBody] SimulateErrorRequest? request)
    {
        var message = request?.Message ?? Messages.Diag_DefaultTestError;

        throw new InvalidOperationException(message);
    }


    [HttpGet("ping")]
    public IActionResult Ping() => Ok(
        new
        {
            status = "ok",
            timestamp = DateTime.UtcNow
        });
}
