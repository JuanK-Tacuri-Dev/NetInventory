namespace NetInventory.Client.Models;

public sealed class ErrorLogModel
{
    public Guid Id { get; set; }
    public string CorrelationId { get; set; } = string.Empty;
    public string ExceptionType { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
}
