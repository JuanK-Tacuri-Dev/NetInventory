namespace NetInventory.Client.Models;

public class StockMovementModel
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Type { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime Timestamp { get; set; }
    public string? CreatedBy { get; set; }
}
