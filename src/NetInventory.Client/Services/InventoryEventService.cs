namespace NetInventory.Client.Services;

public class InventoryEventService
{
    public event Action? OnStockChanged;

    public void NotifyStockChanged() => OnStockChanged?.Invoke();
}
