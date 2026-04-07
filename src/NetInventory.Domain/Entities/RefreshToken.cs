namespace NetInventory.Domain.Entities;

public sealed class RefreshToken
{
    public Guid Id { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public string UserId { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }

    public bool IsValid => !IsRevoked && ExpiresAt > DateTime.UtcNow;

    public static RefreshToken Create(string userId, TimeSpan lifetime) => new()
    {
        Id = Guid.NewGuid(),
        Token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"),
        UserId = userId,
        ExpiresAt = DateTime.UtcNow.Add(lifetime)
    };

    public void Revoke() => IsRevoked = true;
}
