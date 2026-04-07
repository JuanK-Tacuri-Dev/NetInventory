using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;

namespace NetInventory.Application.StockMovements.Queries.GetMovements;

public sealed record GetMovementsQuery(Guid ProductId) : IQuery<Result<IEnumerable<StockMovementDto>>>;
