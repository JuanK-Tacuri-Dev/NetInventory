namespace NetInventory.Domain.Entities;

public sealed class AuditConfig
{
    public Guid Id { get; private set; }
    public string Method { get; private set; } = string.Empty;
    public string UrlPattern { get; private set; } = string.Empty;
    public bool IsEnabled { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private AuditConfig() { }

    public static AuditConfig Create(string method, string urlPattern, string description) => new()
    {
        Id = Guid.NewGuid(),
        Method = method,
        UrlPattern = urlPattern,
        IsEnabled = true,
        Description = description,
        CreatedAt = DateTime.UtcNow
    };

    public void Update(string method, string urlPattern, string description)
    {
        Method = method;
        UrlPattern = urlPattern;
        Description = description;
    }

    public void Toggle() => IsEnabled = !IsEnabled;
}
