namespace NetInventory.Application.Common.DTOs;

public sealed class ProductListItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int CategoryTableId { get; set; }
    public string CategoryCode { get; set; } = string.Empty;
    public string CategoryDescription { get; set; } = string.Empty;
    public int QuantityInStock { get; set; }
    public int MinStock { get; set; }
    public int MaxStock { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}
