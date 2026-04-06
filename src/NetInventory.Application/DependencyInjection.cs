using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NetInventory.Application.Common;
using NetInventory.Application.Products.Commands.CreateProduct;
using NetInventory.Application.Products.Commands.DeleteProduct;
using NetInventory.Application.Products.Commands.UpdateProduct;
using NetInventory.Application.Products.Queries.GetProductById;
using NetInventory.Application.Products.Queries.GetProducts;
using NetInventory.Application.Products.Queries.GetProductsPaged;
using NetInventory.Application.Services;
using NetInventory.Application.StockMovements.Commands.RegisterMovement;
using NetInventory.Application.StockMovements.Queries.GetMovements;

namespace NetInventory.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped(typeof(ValidationBehavior<>));

        services.AddScoped<IMovementStrategy, InboundStrategy>();
        services.AddScoped<IMovementStrategy, OutboundStrategy>();

        services.AddScoped<CreateProductCommandHandler>();
        services.AddScoped<UpdateProductCommandHandler>();
        services.AddScoped<DeleteProductCommandHandler>();

        services.AddScoped<GetProductsQueryHandler>();
        services.AddScoped<GetProductByIdQueryHandler>();
        services.AddScoped<GetProductsPagedQueryHandler>();

        services.AddScoped<RegisterMovementCommandHandler>();
        services.AddScoped<GetMovementsQueryHandler>();

        return services;
    }
}
