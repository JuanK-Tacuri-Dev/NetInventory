using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetInventory.Api.Common.Extensions;
using NetInventory.Application.AuditLogs.Queries.GetAuditLogByCorrelationId;
using NetInventory.Application.AuditLogs.Queries.GetAuditLogs;
using NetInventory.Application.Common;

namespace NetInventory.Api.Controllers;

[ApiController]
[Route("api/audit-logs")]
[Authorize]
public sealed class AuditLogsController(
    IDispatcher dispatcher
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken ct = default)
    {
        var result = await dispatcher.QueryAsync(new GetAuditLogsQuery(page, pageSize), ct);

        return result.ToActionResult(this);
    }

    [HttpGet("{correlationId}")]
    public async Task<IActionResult> GetByCorrelationId(
        string correlationId,
        CancellationToken ct = default)
    {
        var result = await dispatcher.QueryAsync(new GetAuditLogByCorrelationIdQuery(correlationId), ct);

        return result.ToActionResult(this);
    }
}
