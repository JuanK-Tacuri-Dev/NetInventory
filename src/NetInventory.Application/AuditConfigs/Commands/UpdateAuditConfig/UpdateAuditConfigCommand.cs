using NetInventory.Application.Common;
using NetInventory.Domain.Common;

namespace NetInventory.Application.AuditConfigs.Commands.UpdateAuditConfig;

public sealed record UpdateAuditConfigCommand(Guid Id, string Method, string UrlPattern, string Description) : ICommand<Result>;
