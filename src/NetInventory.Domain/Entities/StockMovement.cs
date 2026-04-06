using NetInventory.Domain.Enums;

namespace NetInventory.Domain.Entities;

public sealed class StockMovement
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public MovementType Type { get; private set; }
    public int Quantity { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string? CreatedBy { get; private set; }

    private StockMovement() { }

    public static StockMovement Create(Guid productId, MovementType type, int quantity, string createdBy) => new()
    {
        Id = Guid.NewGuid(),
        ProductId = productId,
        Type = type,
        Quantity = quantity,
        Timestamp = DateTime.UtcNow,
        CreatedBy = createdBy
    };
}
