using Mapster;
using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.AuditConfigs.Commands.CreateAuditConfig;

public sealed class CreateAuditConfigCommandHandler(
    IAuditConfigRepository repository,
    IUnitOfWork unitOfWork,
    IAuditConfigCache cache,
    ValidationBehavior<CreateAuditConfigCommand> validator)
    : ICommandHandler<CreateAuditConfigCommand, Result<AuditConfigDto>>
{
    public async Task<Result<AuditConfigDto>> HandleAsync(
        CreateAuditConfigCommand command, CancellationToken ct = default)
    {
        var validation = validator.Validate(command);

        if (validation.IsFailure)
            return Result.Failure<AuditConfigDto>(validation.Error);

        var config = AuditConfig.Create(
            command.Method.ToUpperInvariant(),
            command.UrlPattern,
            command.Description);

        await repository.AddAsync(config, ct);
        await unitOfWork.SaveChangesAsync(ct);
        cache.Invalidate();

        return Result.Success(config.Adapt<AuditConfigDto>());
    }
}
