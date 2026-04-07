namespace NetInventory.Domain.Entities;

public sealed class AuditLog
{
    public Guid Id { get; private set; }
    public string CorrelationId { get; private set; } = string.Empty;
    public string Method { get; private set; } = string.Empty;
    public string Path { get; private set; } = string.Empty;
    public string? QueryString { get; private set; }
    public string? RequestBody { get; private set; }
    public string? ResponseBody { get; private set; }
    public int StatusCode { get; private set; }
    public long DurationMs { get; private set; }
    public string? UserId { get; private set; }
    public string? UserEmail { get; private set; }
    public DateTime OccurredAt { get; private set; }

    private AuditLog() { }

    public static AuditLog Create(
        string correlationId,
        string method,
        string path,
        string? queryString,
        string? requestBody,
        string? responseBody,
        int statusCode,
        long durationMs,
        string? userId,
        string? userEmail) => new()
    {
        Id = Guid.NewGuid(),
        CorrelationId = correlationId,
        Method = method,
        Path = path,
        QueryString = queryString,
        RequestBody = requestBody,
        ResponseBody = responseBody,
        StatusCode = statusCode,
        DurationMs = durationMs,
        UserId = userId,
        UserEmail = userEmail,
        OccurredAt = DateTime.UtcNow
    };
}
