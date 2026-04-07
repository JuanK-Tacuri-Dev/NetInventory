using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetInventory.Api.Common;
using NetInventory.Api.Common.Extensions;
using NetInventory.Api.Requests.AuditConfigs;
using NetInventory.Application.AuditConfigs.Commands.CreateAuditConfig;
using NetInventory.Application.AuditConfigs.Commands.DeleteAuditConfig;
using NetInventory.Application.AuditConfigs.Commands.ToggleAuditConfig;
using NetInventory.Application.AuditConfigs.Commands.UpdateAuditConfig;
using NetInventory.Application.AuditConfigs.Queries.GetAuditConfigs;
using NetInventory.Application.Common;
using NetInventory.Application.Common.Interfaces;

namespace NetInventory.Api.Controllers;

[ApiController]
[Route("api/audit-configs")]
[Authorize]
public sealed class AuditConfigsController(
    IDispatcher dispatcher,
    IAuditConfigCache cache) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var result = await dispatcher.QueryAsync(new GetAuditConfigsQuery(), ct);

        return result.ToActionResult(this);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateAuditConfigRequest request,
        CancellationToken ct = default)
    {
        var result = await dispatcher.SendAsync(
            new CreateAuditConfigCommand(request.Method, request.UrlPattern, request.Description), ct);

        return result.ToActionResult(this, value => CreatedAtAction(nameof(GetAll), ApiResponse<object>.Ok(value)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAuditConfigRequest request,
        CancellationToken ct = default)
    {
        var result = await dispatcher.SendAsync(
            new UpdateAuditConfigCommand(id, request.Method, request.UrlPattern, request.Description), ct);

        return result.ToActionResult(this);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken ct = default)
    {
        var result = await dispatcher.SendAsync(new DeleteAuditConfigCommand(id), ct);

        return result.ToActionResult(this);
    }

    [HttpPatch("{id:guid}/toggle")]
    public async Task<IActionResult> Toggle(
        Guid id,
        CancellationToken ct = default)
    {
        var result = await dispatcher.SendAsync(new ToggleAuditConfigCommand(id), ct);

        return result.ToActionResult(this, _ => NoContent());
    }

    [HttpDelete("cache")]
    public IActionResult InvalidateCache()
    {
        cache.Invalidate();

        return NoContent();
    }
}
