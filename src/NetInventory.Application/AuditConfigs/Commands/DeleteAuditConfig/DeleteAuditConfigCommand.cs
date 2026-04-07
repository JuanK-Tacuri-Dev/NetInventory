using NetInventory.Application.Common;
using NetInventory.Domain.Common;

namespace NetInventory.Application.AuditConfigs.Commands.DeleteAuditConfig;

public sealed record DeleteAuditConfigCommand(Guid Id) : ICommand<Result>;
