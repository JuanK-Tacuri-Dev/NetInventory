namespace NetInventory.Application.Common.DTOs;

public sealed record AuditConfigDto(
    Guid Id,
    string Method,
    string UrlPattern,
    bool IsEnabled,
    string Description,
    DateTime CreatedAt);
