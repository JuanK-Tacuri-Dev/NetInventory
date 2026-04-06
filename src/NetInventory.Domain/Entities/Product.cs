using NetInventory.Domain.Common;
using NetInventory.Domain.Enums;
using NetInventory.Domain.ValueObjects;

namespace NetInventory.Domain.Entities;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Sku SKU { get; private set; } = default!;
    public string Category { get; private set; } = string.Empty;
    public int QuantityInStock { get; private set; }
    public Money UnitPrice { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public string? UpdatedBy { get; private set; }

    private Product() { }

    public static Product Create(string name, Sku sku, string category, Money unitPrice, string createdBy) => new()
    {
        Id = Guid.NewGuid(),
        Name = name,
        SKU = sku,
        Category = category,
        UnitPrice = unitPrice,
        QuantityInStock = 0,
        CreatedAt = DateTime.UtcNow,
        CreatedBy = createdBy
    };

    public void Update(string name, string category, Money unitPrice, string updatedBy)
    {
        Name = name;
        Category = category;
        UnitPrice = unitPrice;
        UpdatedBy = updatedBy;
    }

    public Result ApplyMovement(int quantity, MovementType type)
    {
        if (quantity <= 0)
            return Result.Failure(Error.InvalidQuantity);

        if (type == MovementType.Outbound && QuantityInStock - quantity < 0)
            return Result.Failure(Error.StockNegative);

        QuantityInStock = type == MovementType.Inbound
            ? QuantityInStock + quantity
            : QuantityInStock - quantity;

        return Result.Success();
    }

    public bool IsLowStock(int threshold = 10) => QuantityInStock < threshold;
}
