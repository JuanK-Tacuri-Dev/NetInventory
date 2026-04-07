using Mapster;
using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.StockMovements.Queries.GetMovements;

public sealed class GetMovementsQueryHandler(
    IProductRepository productRepository,
    IStockMovementRepository stockMovementRepository,
    ICurrentUserService currentUserService)
    : IQueryHandler<GetMovementsQuery, Result<IEnumerable<StockMovementDto>>>
{
    public async Task<Result<IEnumerable<StockMovementDto>>> HandleAsync(GetMovementsQuery query, CancellationToken ct = default)
    {
        var ownerId = currentUserService.GetCurrentUserId();
        var product = await productRepository.GetByIdAsync(query.ProductId, ownerId, ct);
        if (product is null)
            return Result.Failure<IEnumerable<StockMovementDto>>(Error.Product.NotFound);

        var movements = await stockMovementRepository.GetByProductIdAsync(query.ProductId, ct);
        return Result.Success(movements.Adapt<IEnumerable<StockMovementDto>>());
    }
}
