using System.Data;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetInventory.Application.Auth;
using NetInventory.Application.Common;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Interfaces;
using NetInventory.Infrastructure.HostedServices;
using NetInventory.Infrastructure.Persistence;
using NetInventory.Infrastructure.Persistence.ReadModel;
using NetInventory.Infrastructure.Persistence.Repositories;
using NetInventory.Infrastructure.Services;

namespace NetInventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        SqlMapper.AddTypeHandler(new NetInventory.Infrastructure.Persistence.ReadModel.GuidTypeHandler());
        SqlMapper.AddTypeHandler(new NetInventory.Infrastructure.Persistence.ReadModel.DecimalTypeHandler());

        var connectionString = configuration.GetConnectionString("Default")
            ?? "Data Source=inventory.db";

        services.AddDbContext<AppDbContext>(options =>
        {
            if (connectionString.Contains(".db"))
                options.UseSqlite(connectionString).EnableSensitiveDataLogging();
            else
                options.UseSqlServer(connectionString).EnableSensitiveDataLogging();
        });

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = ctx => { ctx.Response.StatusCode = 401; return Task.CompletedTask; };
            options.Events.OnRedirectToAccessDenied = ctx => { ctx.Response.StatusCode = 403; return Task.CompletedTask; };
        });

        services.AddScoped<IDbConnection>(sp =>
        {
            if (connectionString.Contains(".db", StringComparison.OrdinalIgnoreCase))
            {
                var conn = new SqliteConnection(connectionString);
                conn.Open();
                using var wal = conn.CreateCommand();
                wal.CommandText = "PRAGMA journal_mode=WAL;";
                wal.ExecuteNonQuery();
                return conn;
            }
            else
            {
                var conn = new SqlConnection(connectionString);
                conn.Open();
                return conn;
            }
        });
        services.AddScoped<IProductListRepository, ProductListRepository>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IStockMovementRepository, StockMovementRepository>();
        services.AddScoped<IErrorLogRepository, ErrorLogRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IAuditConfigRepository, AuditConfigRepository>();
        services.AddScoped<IGeneralTableRepository, GeneralTableRepository>();
        services.AddScoped<IGeneralValueRepository, GeneralValueRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IDispatcher, Dispatcher>();
        services.AddSingleton<IAuditConfigCache, AuditConfigCache>();
        services.AddSingleton<ICacheService, CacheService>();
        var cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? new CacheSettings();
        services.AddSingleton(cacheSettings);
        services.AddHostedService<LowStockBackgroundService>();
        services.AddHostedService<GeneralValuesCacheWarmupService>();
        services.AddHttpContextAccessor();

        return services;
    }

}
