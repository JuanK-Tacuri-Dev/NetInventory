using System.Data;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetInventory.Application.Common.Interfaces;
using NetInventory.Domain.Interfaces;
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

        var connectionString = configuration.GetConnectionString("Default")
            ?? "Data Source=inventory.db";

        services.AddDbContext<AppDbContext>(options =>
        {
            if (connectionString.Contains(".db"))
                options.UseSqlite(connectionString);
            else
                options.UseSqlServer(connectionString);
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
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHostedService<LowStockBackgroundService>();
        services.AddHttpContextAccessor();

        return services;
    }
}
