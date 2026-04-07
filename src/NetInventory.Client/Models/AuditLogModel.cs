namespace NetInventory.Client.Models;

public class AuditLogModel
{
    public Guid Id { get; set; }
    public string CorrelationId { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string? QueryString { get; set; }
    public string? RequestBody { get; set; }
    public string? ResponseBody { get; set; }
    public int StatusCode { get; set; }
    public long DurationMs { get; set; }
    public string? UserId { get; set; }
    public string? UserEmail { get; set; }
    public DateTime OccurredAt { get; set; }
}
