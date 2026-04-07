using Mapster;
using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.AuditConfigs.Commands.ToggleAuditConfig;

public sealed class ToggleAuditConfigCommandHandler(
    IAuditConfigRepository repository,
    IUnitOfWork unitOfWork,
    IAuditConfigCache cache)
    : ICommandHandler<ToggleAuditConfigCommand, Result<AuditConfigDto>>
{
    public async Task<Result<AuditConfigDto>> HandleAsync(
        ToggleAuditConfigCommand command, CancellationToken ct = default)
    {
        var config = await repository.GetByIdAsync(command.Id, ct);
        if (config is null)
            return Result.Failure<AuditConfigDto>(Error.AuditConfig.NotFound);

        config.Toggle();
        await repository.UpdateAsync(config, ct);
        await unitOfWork.SaveChangesAsync(ct);
        cache.Invalidate();

        return Result.Success(config.Adapt<AuditConfigDto>());
    }
}
