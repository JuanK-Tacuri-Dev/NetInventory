using NetInventory.Application.Auth.Dtos;
using NetInventory.Application.Common;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;

namespace NetInventory.Application.Auth.Commands.Refresh;

public sealed class RefreshCommandHandler(
    IRefreshTokenRepository refreshTokenRepository,
    IUserRepository userRepository,
    ITokenService tokenService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RefreshCommand, Result<AuthTokenDto>>
{
    public async Task<Result<AuthTokenDto>> HandleAsync(RefreshCommand command, CancellationToken ct = default)
    {
        var stored = await refreshTokenRepository.GetByTokenAsync(command.RefreshToken, ct);
        if (stored is null || !stored.IsValid)
            return Result.Failure<AuthTokenDto>(Error.Auth.InvalidRefreshToken);

        var userResult = await userRepository.FindByIdAsync(stored.UserId, ct);
        if (userResult.IsFailure)
            return Result.Failure<AuthTokenDto>(userResult.Error);

        stored.Revoke();
        var newRefreshToken = RefreshToken.Create(stored.UserId, Constants.Auth.RefreshTokenLifetime);
        await refreshTokenRepository.AddAsync(newRefreshToken);
        await unitOfWork.SaveChangesAsync(ct);

        var (token, expiresAt) = tokenService.GenerateToken(userResult.Value.UserId, userResult.Value.Email);

        return Result.Success(new AuthTokenDto(token, expiresAt, newRefreshToken.Token, newRefreshToken.ExpiresAt));
    }
}
