namespace NetInventory.Api.Requests.AuditConfigs;

public sealed record UpdateAuditConfigRequest(string Method, string UrlPattern, string Description);
