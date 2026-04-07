using Mapster;
using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.GeneralTables.Queries.GetGeneralTables;

public sealed class GetGeneralTablesQueryHandler(IGeneralTableRepository repository)
    : IQueryHandler<GetGeneralTablesQuery, Result<IEnumerable<GeneralTableDto>>>
{
    public async Task<Result<IEnumerable<GeneralTableDto>>> HandleAsync(
        GetGeneralTablesQuery query, CancellationToken ct = default)
    {
        var tables = await repository.GetAllAsync(ct);
        return Result.Success(tables.Adapt<IEnumerable<GeneralTableDto>>());
    }
}
