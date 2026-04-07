namespace NetInventory.Domain.Entities;

public sealed class ErrorLog
{
    public Guid Id { get; private set; }
    public string ReferenceCode { get; private set; } = string.Empty;
    public string CorrelationId { get; private set; } = string.Empty;
    public string ExceptionType { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public string? StackTrace { get; private set; }
    public string Path { get; private set; } = string.Empty;
    public string Method { get; private set; } = string.Empty;
    public DateTime OccurredAt { get; private set; }

    private ErrorLog() { }

    public static ErrorLog Create(string correlationId, Exception ex, string path, string method) => new()
    {
        Id = Guid.NewGuid(),
        ReferenceCode = "ERR-" + Guid.NewGuid().ToString("N")[..6].ToUpper(),
        CorrelationId = correlationId,
        ExceptionType = ex.GetType().Name,
        Message = ex.Message,
        StackTrace = ex.StackTrace,
        Path = path,
        Method = method,
        OccurredAt = DateTime.UtcNow
    };
}
