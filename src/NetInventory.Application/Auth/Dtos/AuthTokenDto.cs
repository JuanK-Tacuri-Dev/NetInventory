namespace NetInventory.Application.Auth.Dtos;

public sealed record AuthTokenDto(
    string Token,
    DateTime ExpiresAt,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt);
