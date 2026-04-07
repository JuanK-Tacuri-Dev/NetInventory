using Microsoft.EntityFrameworkCore;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Interfaces;
using NetInventory.Infrastructure.Persistence;

namespace NetInventory.Infrastructure.Persistence.Repositories;

public sealed class RefreshTokenRepository(AppDbContext db) : IRefreshTokenRepository
{
    public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default)
        => db.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token, ct);

    public async Task AddAsync(RefreshToken refreshToken)
        => await db.RefreshTokens.AddAsync(refreshToken);
}
