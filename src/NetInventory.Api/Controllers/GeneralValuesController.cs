using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetInventory.Api.Common.Extensions;
using NetInventory.Application.Common;
using NetInventory.Application.GeneralTables.Queries.GetGeneralTables;
using NetInventory.Application.GeneralValues.Queries.GetGeneralValuesByTable;

namespace NetInventory.Api.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public sealed class GeneralValuesController(
    IDispatcher dispatcher
    ) : ControllerBase
{
    
    [HttpGet("general-values")]
    public async Task<IActionResult> GetByTable(
        [FromQuery] int tableId,
        CancellationToken ct = default)
    {
        var result = await dispatcher.QueryAsync(new GetGeneralValuesByTableQuery(tableId), ct);

        return result.ToActionResult(this);
    }

    [HttpGet("general-tables")]
    public async Task<IActionResult> GetTables(
        CancellationToken ct = default)
    {
        var result = await dispatcher.QueryAsync(new GetGeneralTablesQuery(), ct);

        return result.ToActionResult(this);
    }
}
