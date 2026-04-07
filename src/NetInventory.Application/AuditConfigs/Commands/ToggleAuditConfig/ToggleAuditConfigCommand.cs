using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;

namespace NetInventory.Application.AuditConfigs.Commands.ToggleAuditConfig;

public sealed record ToggleAuditConfigCommand(Guid Id) : ICommand<Result<AuditConfigDto>>;
