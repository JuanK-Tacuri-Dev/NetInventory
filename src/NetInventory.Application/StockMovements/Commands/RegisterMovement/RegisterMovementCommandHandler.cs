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
{
    public async Task<Result<StockMovementDto>> HandleAsync(RegisterMovementCommand command, CancellationToken ct = default)
    {
        var validation = validator.Validate(command);
        if (validation.IsFailure)
            return Result.Failure<StockMovementDto>(validation.Error);

        var product = await productRepository.GetByIdAsync(command.ProductId, ct);
        if (product is null)
            return Result.Failure<StockMovementDto>(Error.ProductNotFound);

        if (!Enum.TryParse<MovementType>(command.Type, out var movementType))
            return Result.Failure<StockMovementDto>(new Error("INVALID_TYPE", "Tipo de movimiento no reconocido."));

        var strategy = strategies.FirstOrDefault(s => s.MovementType == movementType);
        if (strategy is null)
            return Result.Failure<StockMovementDto>(new Error("STRATEGY_NOT_FOUND", "No existe estrategia para el tipo de movimiento."));

        var applyResult = strategy.Apply(product, command.Quantity);
        if (applyResult.IsFailure)
            return Result.Failure<StockMovementDto>(applyResult.Error);

        var currentUser = currentUserService.GetCurrentUser();
        var movement = StockMovement.Create(product.Id, movementType, command.Quantity, currentUser);

        await productRepository.UpdateAsync(product, ct);
        await stockMovementRepository.AddAsync(movement, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success(ToDto(movement));
    }

    private static StockMovementDto ToDto(StockMovement m) =>
        new(m.Id, m.ProductId, m.Type.ToString(), m.Quantity, m.Timestamp, m.CreatedBy);
}
