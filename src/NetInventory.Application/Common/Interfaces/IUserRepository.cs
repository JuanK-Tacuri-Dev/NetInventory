using NetInventory.Application.Auth.Dtos;
using NetInventory.Domain.Common;

namespace NetInventory.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<Result> CreateAsync(string email, string password, CancellationToken ct = default);
    Task<Result<UserIdentityDto>> ValidateCredentialsAsync(string email, string password, CancellationToken ct = default);
    Task<Result<UserIdentityDto>> FindByIdAsync(string userId, CancellationToken ct = default);
}
