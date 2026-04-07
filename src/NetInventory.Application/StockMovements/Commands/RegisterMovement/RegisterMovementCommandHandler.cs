using Mapster;
using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Application.Services;
using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Enums;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.StockMovements.Commands.RegisterMovement;

public sealed class RegisterMovementCommandHandler(
    IProductRepository productRepository,
    IStockMovementRepository stockMovementRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    ValidationBehavior<RegisterMovementCommand> validator,
    IEnumerable<IMovementStrategy> strategies)
    : ICommandHandler<RegisterMovementCommand, Result<StockMovementDto>>
{
    public async Task<Result<StockMovementDto>> HandleAsync(RegisterMovementCommand command, CancellationToken ct = default)
    {
        var validation = validator.Validate(command);
        if (validation.IsFailure)
            return Result.Failure<StockMovementDto>(validation.Error);

        var ownerId = currentUserService.GetCurrentUserId();
        var product = await productRepository.GetByIdAsync(command.ProductId, ownerId, ct);
        if (product is null)
            return Result.Failure<StockMovementDto>(Error.Product.NotFound);

        if (!Enum.TryParse<MovementType>(command.Type, out var movementType))
            return Result.Failure<StockMovementDto>(Error.Stock.InvalidMovementType);

        var strategy = strategies.FirstOrDefault(s => s.MovementType == movementType);
        if (strategy is null)
            return Result.Failure<StockMovementDto>(Error.Stock.StrategyNotFound);

        var applyResult = strategy.Apply(product, command.Quantity);
        if (applyResult.IsFailure)
            return Result.Failure<StockMovementDto>(applyResult.Error);

        var currentUser = currentUserService.GetCurrentUser();
        var movement = StockMovement.Create(product.Id, movementType, command.Quantity, command.Reason, currentUser);

        await productRepository.UpdateAsync(product, ct);
        await stockMovementRepository.AddAsync(movement, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success(movement.Adapt<StockMovementDto>());
    }
}
