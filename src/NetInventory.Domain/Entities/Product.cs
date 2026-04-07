using NetInventory.Domain.Common;
using NetInventory.Domain.Enums;
using NetInventory.Domain.ValueObjects;

namespace NetInventory.Domain.Entities;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public Sku SKU { get; private set; } = default!;
    public int CategoryTableId { get; private set; }
    public string CategoryCode { get; private set; } = string.Empty;
    public int QuantityInStock { get; private set; }
    public int MinStock { get; private set; }
    public int MaxStock { get; private set; }
    public Money UnitPrice { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public string? UpdatedBy { get; private set; }
    public string OwnerId { get; private set; } = string.Empty;

    private Product() { }

    public static Product Create(string name, Sku sku, int categoryTableId, string categoryCode, Money unitPrice, int minStock, int maxStock, string createdBy, string ownerId) => new()
    {
        Id = Guid.NewGuid(),
        Name = name,
        SKU = sku,
        CategoryTableId = categoryTableId,
        CategoryCode = categoryCode,
        UnitPrice = unitPrice,
        QuantityInStock = 0,
        MinStock = minStock,
        MaxStock = maxStock,
        CreatedAt = DateTime.UtcNow,
        CreatedBy = createdBy,
        OwnerId = ownerId
    };

    public void Update(string name, int categoryTableId, string categoryCode, Money unitPrice, int minStock, int maxStock, string updatedBy)
    {
        Name = name;
        CategoryTableId = categoryTableId;
        CategoryCode = categoryCode;
        UnitPrice = unitPrice;
        MinStock = minStock;
        MaxStock = maxStock;
        UpdatedBy = updatedBy;
    }

    public Result ApplyMovement(int quantity, MovementType type)
    {
        if (quantity <= 0)
            return Result.Failure(Error.Stock.InvalidQuantity);

        if (type == MovementType.Outbound && QuantityInStock - quantity < 0)
            return Result.Failure(Error.Stock.Negative);

        QuantityInStock = type == MovementType.Inbound
            ? QuantityInStock + quantity
            : QuantityInStock - quantity;

        return Result.Success();
    }

    public bool IsLowStock() => QuantityInStock < MinStock;
}
