using NetInventory.Domain.Entities;
using NetInventory.Domain.Enums;
using NetInventory.Domain.ValueObjects;

namespace NetInventory.UnitTests.Application;

internal static class ApplicationTestHelpers
{
    internal static Product CreateProduct(string sku = "TEST-001", int stock = 0)
    {
        var skuVo = Sku.Create(sku).Value;
        var price = Money.Create(10m).Value;
        var product = Product.Create("Test", skuVo, "Cat", price, "user");
        if (stock > 0) product.ApplyMovement(stock, MovementType.Inbound);
        return product;
    }
}
