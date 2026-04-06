using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.StockMovements.Queries.GetMovements;

public sealed class GetMovementsQueryHandler(
    IProductRepository productRepository,
    IStockMovementRepository stockMovementRepository)
{
    public async Task<Result<IEnumerable<StockMovementDto>>> HandleAsync(GetMovementsQuery query, CancellationToken ct = default)
    {
        var product = await productRepository.GetByIdAsync(query.ProductId, ct);
        if (product is null)
            return Result.Failure<IEnumerable<StockMovementDto>>(Error.ProductNotFound);

        var movements = await stockMovementRepository.GetByProductIdAsync(query.ProductId, ct);
        return Result.Success(movements.Select(ToDto));
    }

    private static StockMovementDto ToDto(StockMovement m) =>
        new(m.Id, m.ProductId, m.Type.ToString(), m.Quantity, m.Timestamp, m.CreatedBy);
}
