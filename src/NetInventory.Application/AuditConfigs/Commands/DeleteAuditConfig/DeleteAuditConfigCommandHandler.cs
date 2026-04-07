using NetInventory.Application.Common;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.AuditConfigs.Commands.DeleteAuditConfig;

public sealed class DeleteAuditConfigCommandHandler(
    IAuditConfigRepository repository,
    IUnitOfWork unitOfWork,
    IAuditConfigCache cache)
    : ICommandHandler<DeleteAuditConfigCommand, Result>
{
    public async Task<Result> HandleAsync(DeleteAuditConfigCommand command, CancellationToken ct = default)
    {
        var config = await repository.GetByIdAsync(command.Id, ct);
        if (config is null)
            return Result.Failure(Error.AuditConfig.NotFound);

        await repository.DeleteAsync(config, ct);
        await unitOfWork.SaveChangesAsync(ct);
        cache.Invalidate();

        return Result.Success();
    }
}
