namespace NetInventory.Api.Requests.Products;

public sealed record RegisterMovementRequest(string Type, int Quantity, string? Reason);
