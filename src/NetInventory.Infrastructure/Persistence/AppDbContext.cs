using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetInventory.Domain.Entities;

namespace NetInventory.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<ErrorLog> ErrorLogs => Set<ErrorLog>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<AuditConfig> AuditConfigs => Set<AuditConfig>();
    public DbSet<GeneralTable> GeneralTables => Set<GeneralTable>();
    public DbSet<GeneralValue> GeneralValues => Set<GeneralValue>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityUser>().ToTable("tbUsers");
        builder.Entity<IdentityRole>().ToTable("tbRoles");
        builder.Entity<IdentityUserRole<string>>().ToTable("tbUserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("tbUserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("tbUserLogins");
        builder.Entity<IdentityUserToken<string>>().ToTable("tbUserTokens");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("tbRoleClaims");

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
