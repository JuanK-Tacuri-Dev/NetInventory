using NetInventory.Application.Common.Mappings;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Enums;
using NetInventory.Domain.ValueObjects;

namespace NetInventory.UnitTests.Application;

internal static class ApplicationTestHelpers
{
    static ApplicationTestHelpers() => MappingConfig.RegisterMappings();

    internal static Product CreateProduct(string sku = "TEST-001", int stock = 0)
    {
        var skuVo = Sku.Create(sku).Value;
        var price = Money.Create(10m).Value;
        var product = Product.Create("Test", skuVo, 1001, "001", price, 5, 100, "user", "owner-1");
        if (stock > 0) product.ApplyMovement(stock, MovementType.Inbound);
        return product;
    }
}
