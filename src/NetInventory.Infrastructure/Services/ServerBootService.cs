namespace NetInventory.Infrastructure.Services;


public sealed class ServerBootService
{
    public Guid BootId { get; } = Guid.NewGuid();
}
