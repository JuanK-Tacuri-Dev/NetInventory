using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;

namespace NetInventory.Application.StockMovements.Commands.RegisterMovement;

public sealed record RegisterMovementCommand(
    Guid ProductId,
    string Type,
    int Quantity,
    string? Reason) : ICommand<Result<StockMovementDto>>;
