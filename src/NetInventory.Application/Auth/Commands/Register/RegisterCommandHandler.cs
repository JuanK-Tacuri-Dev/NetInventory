using NetInventory.Application.Common;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;

namespace NetInventory.Application.Auth.Commands.Register;

public sealed class RegisterCommandHandler(IUserRepository userRepository)
    : ICommandHandler<RegisterCommand, Result>
{
    public Task<Result> HandleAsync(RegisterCommand command, CancellationToken ct = default)
        => userRepository.CreateAsync(command.Email, command.Password, ct);
}
