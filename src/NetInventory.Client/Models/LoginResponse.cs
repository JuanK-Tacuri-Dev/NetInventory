namespace NetInventory.Client.Models;

public record LoginResponse(
    string Token,
    DateTime ExpiresAt,
    string? RefreshToken,
    DateTime? RefreshTokenExpiresAt);
