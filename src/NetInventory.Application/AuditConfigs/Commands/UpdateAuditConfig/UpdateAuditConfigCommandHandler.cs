using NetInventory.Application.Common;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.AuditConfigs.Commands.UpdateAuditConfig;

public sealed class UpdateAuditConfigCommandHandler(
    IAuditConfigRepository repository,
    IUnitOfWork unitOfWork,
    IAuditConfigCache cache,
    ValidationBehavior<UpdateAuditConfigCommand> validator)
    : ICommandHandler<UpdateAuditConfigCommand, Result>
{
    public async Task<Result> HandleAsync(UpdateAuditConfigCommand command, CancellationToken ct = default)
    {
        var validation = validator.Validate(command);
        if (validation.IsFailure)
            return validation;

        var config = await repository.GetByIdAsync(command.Id, ct);
        if (config is null)
            return Result.Failure(Error.AuditConfig.NotFound);

        config.Update(command.Method.ToUpperInvariant(), command.UrlPattern, command.Description);

        await repository.UpdateAsync(config, ct);
        await unitOfWork.SaveChangesAsync(ct);
        cache.Invalidate();

        return Result.Success();
    }
}
