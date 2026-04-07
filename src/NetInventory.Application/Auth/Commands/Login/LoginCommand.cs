using NetInventory.Application.Auth.Dtos;
using NetInventory.Application.Common;
using NetInventory.Domain.Common;

namespace NetInventory.Application.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<Result<AuthTokenDto>>;
