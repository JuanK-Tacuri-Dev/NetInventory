using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;

namespace NetInventory.Application.GeneralTables.Queries.GetGeneralTables;

public sealed record GetGeneralTablesQuery() : IQuery<Result<IEnumerable<GeneralTableDto>>>;
