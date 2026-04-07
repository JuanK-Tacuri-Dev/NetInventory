using NetInventory.Application.Common;
using NetInventory.Domain.Common;

namespace NetInventory.Application.Auth.Commands.Register;

public sealed record RegisterCommand(string Email, string Password) : ICommand<Result>;
