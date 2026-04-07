using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NetInventory.Application.Auth.Commands.Login;
using NetInventory.Application.Auth.Commands.Refresh;
using NetInventory.Application.Auth.Commands.Register;
using NetInventory.Application.Auth.Dtos;
using NetInventory.Application.Common;
using NetInventory.Application.Common.DTOs;
using NetInventory.Application.Common.Mappings;
using NetInventory.Application.Products.Commands.CreateProduct;
using NetInventory.Application.Products.Commands.DeleteProduct;
using NetInventory.Application.Products.Commands.UpdateProduct;
using NetInventory.Application.Products.Queries.GetProductById;
using NetInventory.Application.Products.Queries.GetProducts;
using NetInventory.Application.Products.Queries.GetProductsPaged;
using NetInventory.Application.Services;
using NetInventory.Application.StockMovements.Commands.RegisterMovement;
using NetInventory.Application.StockMovements.Queries.GetMovements;
using NetInventory.Application.ErrorLogs.Queries.GetErrorLogs;
using NetInventory.Application.AuditLogs.Queries.GetAuditLogs;
using NetInventory.Application.AuditLogs.Queries.GetAuditLogByCorrelationId;
using NetInventory.Application.AuditConfigs.Queries.GetAuditConfigs;
using NetInventory.Application.AuditConfigs.Commands.CreateAuditConfig;
using NetInventory.Application.AuditConfigs.Commands.UpdateAuditConfig;
using NetInventory.Application.AuditConfigs.Commands.DeleteAuditConfig;
using NetInventory.Application.AuditConfigs.Commands.ToggleAuditConfig;
using NetInventory.Application.GeneralValues.Queries.GetGeneralValuesByTable;
using NetInventory.Application.GeneralTables.Queries.GetGeneralTables;
using NetInventory.Domain.Common;

namespace NetInventory.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        MappingConfig.RegisterMappings();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped(typeof(ValidationBehavior<>));

        services.AddScoped<IMovementStrategy, InboundStrategy>();
        services.AddScoped<IMovementStrategy, OutboundStrategy>();

        // Auth command handlers
        services.AddScoped<ICommandHandler<RegisterCommand, Result>, RegisterCommandHandler>();
        services.AddScoped<ICommandHandler<LoginCommand, Result<AuthTokenDto>>, LoginCommandHandler>();
        services.AddScoped<ICommandHandler<RefreshCommand, Result<AuthTokenDto>>, RefreshCommandHandler>();

        // Command handlers
        services.AddScoped<ICommandHandler<CreateProductCommand, Result<ProductDto>>, CreateProductCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateProductCommand, Result>, UpdateProductCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteProductCommand, Result>, DeleteProductCommandHandler>();
        services.AddScoped<ICommandHandler<RegisterMovementCommand, Result<StockMovementDto>>, RegisterMovementCommandHandler>();
        services.AddScoped<ICommandHandler<CreateAuditConfigCommand, Result<AuditConfigDto>>, CreateAuditConfigCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateAuditConfigCommand, Result>, UpdateAuditConfigCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteAuditConfigCommand, Result>, DeleteAuditConfigCommandHandler>();
        services.AddScoped<ICommandHandler<ToggleAuditConfigCommand, Result<AuditConfigDto>>, ToggleAuditConfigCommandHandler>();

        // Query handlers
        services.AddScoped<IQueryHandler<GetProductsQuery, Result<IEnumerable<ProductDto>>>, GetProductsQueryHandler>();
        services.AddScoped<IQueryHandler<GetProductByIdQuery, Result<ProductDto>>, GetProductByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetProductsPagedQuery, Result<PagedResult<ProductListItem>>>, GetProductsPagedQueryHandler>();
        services.AddScoped<IQueryHandler<GetMovementsQuery, Result<IEnumerable<StockMovementDto>>>, GetMovementsQueryHandler>();
        services.AddScoped<IQueryHandler<GetErrorLogsQuery, PagedResult<ErrorLogDto>>, GetErrorLogsQueryHandler>();
        services.AddScoped<IQueryHandler<GetAuditLogsQuery, Result<PagedResult<AuditLogDto>>>, GetAuditLogsQueryHandler>();
        services.AddScoped<IQueryHandler<GetAuditLogByCorrelationIdQuery, Result<AuditLogDto>>, GetAuditLogByCorrelationIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetAuditConfigsQuery, Result<IEnumerable<AuditConfigDto>>>, GetAuditConfigsQueryHandler>();
        services.AddScoped<IQueryHandler<GetGeneralValuesByTableQuery, Result<IEnumerable<GeneralValueDto>>>, GetGeneralValuesByTableQueryHandler>();
        services.AddScoped<IQueryHandler<GetGeneralTablesQuery, Result<IEnumerable<GeneralTableDto>>>, GetGeneralTablesQueryHandler>();

        return services;
    }
}
