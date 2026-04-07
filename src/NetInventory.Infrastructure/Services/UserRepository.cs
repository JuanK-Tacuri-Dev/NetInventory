using Microsoft.AspNetCore.Identity;
using NetInventory.Application.Auth.Dtos;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Common;

namespace NetInventory.Infrastructure.Services;

public sealed class UserRepository(UserManager<IdentityUser> userManager) : IUserRepository
{
    public async Task<Result> CreateAsync(string email, string password, CancellationToken ct = default)
    {
        var user = new IdentityUser { UserName = email, Email = email };
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var detail = string.Join("; ", result.Errors.Select(e => e.Description));
            return Result.Failure(Error.Auth.RegistrationFailed(detail));
        }
        return Result.Success();
    }

    public async Task<Result<UserIdentityDto>> ValidateCredentialsAsync(string email, string password, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result.Failure<UserIdentityDto>(Error.Auth.InvalidCredentials);

        var valid = await userManager.CheckPasswordAsync(user, password);
        if (!valid)
            return Result.Failure<UserIdentityDto>(Error.Auth.InvalidCredentials);

        return Result.Success(new UserIdentityDto(user.Id, user.Email!));
    }

    public async Task<Result<UserIdentityDto>> FindByIdAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure<UserIdentityDto>(Error.Auth.UserNotFound);

        return Result.Success(new UserIdentityDto(user.Id, user.Email!));
    }
}
