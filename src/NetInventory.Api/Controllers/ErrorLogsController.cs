using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetInventory.Api.Common;
using NetInventory.Application.Common;
using NetInventory.Application.ErrorLogs.Queries.GetErrorLogs;

namespace NetInventory.Api.Controllers;

[ApiController]
[Route("api/error-logs")]
[Authorize]
public sealed class ErrorLogsController(
    IDispatcher dispatcher
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 200,
        CancellationToken ct = default)
    {
        var result = await dispatcher.QueryAsync(new GetErrorLogsQuery(page, pageSize), ct);

        return Ok(ApiResponse<object>.Ok(result));
    }
}
