namespace NetInventory.Application.Common.DTOs;

public sealed record AuditLogDto(
    Guid Id,
    string CorrelationId,
    string Method,
    string Path,
    string? QueryString,
    string? RequestBody,
    string? ResponseBody,
    int StatusCode,
    long DurationMs,
    string? UserId,
    string? UserEmail,
    DateTime OccurredAt);
