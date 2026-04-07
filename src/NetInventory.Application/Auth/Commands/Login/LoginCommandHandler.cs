using NetInventory.Application.Auth.Dtos;
using NetInventory.Application.Common;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    ITokenService tokenService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<LoginCommand, Result<AuthTokenDto>>
{
    public async Task<Result<AuthTokenDto>> HandleAsync(LoginCommand command, CancellationToken ct = default)
    {
        var userResult = await userRepository.ValidateCredentialsAsync(command.Email, command.Password, ct);
        if (userResult.IsFailure)
            return Result.Failure<AuthTokenDto>(userResult.Error);

        var user = userResult.Value;
        var refreshToken = RefreshToken.Create(user.UserId, Constants.Auth.RefreshTokenLifetime);
        await refreshTokenRepository.AddAsync(refreshToken);
        await unitOfWork.SaveChangesAsync(ct);

        var (token, expiresAt) = tokenService.GenerateToken(user.UserId, user.Email);

        return Result.Success(new AuthTokenDto(token, expiresAt, refreshToken.Token, refreshToken.ExpiresAt));
    }
}
