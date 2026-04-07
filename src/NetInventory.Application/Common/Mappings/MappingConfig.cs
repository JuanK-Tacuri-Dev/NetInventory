using Mapster;
using NetInventory.Application.Common.DTOs;
using NetInventory.Domain.Entities;

namespace NetInventory.Application.Common.Mappings;

public static class MappingConfig
{
    private static bool _registered;

    public static void RegisterMappings()
    {
        if (_registered) return;
        _registered = true;

        // Product → ProductDto
        TypeAdapterConfig<Product, ProductDto>.NewConfig()
            .Map(dest => dest.SKU,                src => src.SKU.Value)
            .Map(dest => dest.UnitPrice,          src => src.UnitPrice.Amount)
            .Map(dest => dest.IsLowStock,         src => src.IsLowStock())
            .Map(dest => dest.CategoryDescription, src => string.Empty);

        // GeneralTable / GeneralValue → DTOs
        TypeAdapterConfig<GeneralTable, GeneralTableDto>.NewConfig();
        TypeAdapterConfig<GeneralValue, GeneralValueDto>.NewConfig();

        // StockMovement → StockMovementDto
        // Enum → string para el campo Type
        TypeAdapterConfig<StockMovement, StockMovementDto>.NewConfig()
            .Map(dest => dest.Type, src => src.Type.ToString());

        // AuditConfig → AuditConfigDto  (todos los campos coinciden 1:1)
        TypeAdapterConfig<AuditConfig, AuditConfigDto>.NewConfig();

        // AuditLog → AuditLogDto  (todos los campos coinciden 1:1)
        TypeAdapterConfig<AuditLog, AuditLogDto>.NewConfig();

        // ErrorLog → ErrorLogDto  (todos los campos coinciden 1:1)
        TypeAdapterConfig<ErrorLog, ErrorLogDto>.NewConfig();
    }
}
