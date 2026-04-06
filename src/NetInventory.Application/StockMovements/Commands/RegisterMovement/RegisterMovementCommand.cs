namespace NetInventory.Application.StockMovements.Commands.RegisterMovement;

public sealed record RegisterMovementCommand(
    Guid ProductId,
    string Type,
    int Quantity);
