using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;

namespace NetInventory.Application.GeneralValues.Queries.GetGeneralValuesByTable;

public sealed record GetGeneralValuesByTableQuery(int TableId) : IQuery<Result<IEnumerable<GeneralValueDto>>>;
