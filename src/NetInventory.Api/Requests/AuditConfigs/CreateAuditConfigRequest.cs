namespace NetInventory.Api.Requests.AuditConfigs;

public sealed record CreateAuditConfigRequest(string Method, string UrlPattern, string Description);
