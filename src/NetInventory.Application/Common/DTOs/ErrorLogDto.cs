namespace NetInventory.Application.Common.DTOs;

public sealed record ErrorLogDto(
    Guid Id,
    string CorrelationId,
    string ExceptionType,
    string Message,
    string? StackTrace,
    string Path,
    string Method,
    DateTime OccurredAt);
