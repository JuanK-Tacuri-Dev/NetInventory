using NetInventory.Application.Auth.Dtos;
using NetInventory.Application.Common;
using NetInventory.Domain.Common;

namespace NetInventory.Application.Auth.Commands.Refresh;

public sealed record RefreshCommand(string RefreshToken) : ICommand<Result<AuthTokenDto>>;
