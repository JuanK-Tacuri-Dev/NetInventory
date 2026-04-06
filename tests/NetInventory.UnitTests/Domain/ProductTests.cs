using FluentAssertions;
using NetInventory.Domain.Common;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Enums;
using NetInventory.Domain.ValueObjects;

namespace NetInventory.UnitTests.Domain;

public class ProductTests
{
    private static Product CreateValidProduct(int initialStock = 0)
    {
        var sku = Sku.Create("TEST-001").Value;
        var price = Money.Create(99.99m).Value;
        var product = Product.Create("Test Product", sku, "Electronics", price, "user1");
        if (initialStock > 0)
            product.ApplyMovement(initialStock, MovementType.Inbound);
        return product;
    }

    [Fact]
    public void Create_WithValidData_ReturnsProductWithZeroStock()
    {
        var product = CreateValidProduct();

        product.QuantityInStock.Should().Be(0);
        product.Name.Should().Be("Test Product");
        product.Category.Should().Be("Electronics");
        product.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Create_SetsCreatedByCorrectly()
    {
        var sku = Sku.Create("SKU-001").Value;
        var price = Money.Create(10m).Value;

        var product = Product.Create("Item", sku, "Cat", price, "admin");

        product.CreatedBy.Should().Be("admin");
    }

    [Fact]
    public void ApplyMovement_Inbound_IncreasesStock()
    {
        var product = CreateValidProduct();

        var result = product.ApplyMovement(10, MovementType.Inbound);

        result.IsSuccess.Should().BeTrue();
        product.QuantityInStock.Should().Be(10);
    }

    [Fact]
    public void ApplyMovement_Outbound_DecreasesStock()
    {
        var product = CreateValidProduct(initialStock: 20);

        var result = product.ApplyMovement(5, MovementType.Outbound);

        result.IsSuccess.Should().BeTrue();
        product.QuantityInStock.Should().Be(15);
    }

    [Fact]
    public void ApplyMovement_OutboundExceedingStock_ReturnsStockNegativeError()
    {
        var product = CreateValidProduct(initialStock: 5);

        var result = product.ApplyMovement(10, MovementType.Outbound);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.StockNegative);
    }

    [Fact]
    public void ApplyMovement_WithZeroQuantity_ReturnsInvalidQuantityError()
    {
        var product = CreateValidProduct();

        var result = product.ApplyMovement(0, MovementType.Inbound);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.InvalidQuantity);
    }

    [Fact]
    public void ApplyMovement_WithNegativeQuantity_ReturnsInvalidQuantityError()
    {
        var product = CreateValidProduct();

        var result = product.ApplyMovement(-5, MovementType.Inbound);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(Error.InvalidQuantity);
    }

    [Fact]
    public void IsLowStock_WhenStockBelowThreshold_ReturnsTrue()
    {
        var product = CreateValidProduct(initialStock: 5);

        product.IsLowStock(threshold: 10).Should().BeTrue();
    }

    [Fact]
    public void IsLowStock_WhenStockAboveThreshold_ReturnsFalse()
    {
        var product = CreateValidProduct(initialStock: 15);

        product.IsLowStock(threshold: 10).Should().BeFalse();
    }

    [Fact]
    public void IsLowStock_WhenStockEqualsThreshold_ReturnsFalse()
    {
        var product = CreateValidProduct(initialStock: 10);

        product.IsLowStock(threshold: 10).Should().BeFalse();
    }

    [Fact]
    public void Update_ChangesNameCategoryPriceAndUpdatedBy()
    {
        var product = CreateValidProduct();
        var newPrice = Money.Create(199.99m).Value;

        product.Update("Updated Name", "NewCategory", newPrice, "editor");

        product.Name.Should().Be("Updated Name");
        product.Category.Should().Be("NewCategory");
        product.UnitPrice.Amount.Should().Be(199.99m);
        product.UpdatedBy.Should().Be("editor");
    }
}
