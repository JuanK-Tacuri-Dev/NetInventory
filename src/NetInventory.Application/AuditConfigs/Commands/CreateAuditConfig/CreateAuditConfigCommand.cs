using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Common;

namespace NetInventory.Application.AuditConfigs.Commands.CreateAuditConfig;

public sealed record CreateAuditConfigCommand(string Method, string UrlPattern, string Description) : ICommand<Result<AuditConfigDto>>;
