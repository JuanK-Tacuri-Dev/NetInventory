using FluentAssertions;
using NetInventory.Domain.Entities;
using NetInventory.Domain.Enums;

namespace NetInventory.UnitTests.Domain;

public class StockMovementTests
{
    [Fact]
    public void Create_WithValidData_SetsAllPropertiesCorrectly()
    {
        var productId = Guid.NewGuid();

        var movement = StockMovement.Create(productId, MovementType.Inbound, 50, null, "warehouse");

        movement.Id.Should().NotBe(Guid.Empty);
        movement.ProductId.Should().Be(productId);
        movement.Type.Should().Be(MovementType.Inbound);
        movement.Quantity.Should().Be(50);
        movement.CreatedBy.Should().Be("warehouse");
    }

    [Fact]
    public void Create_SetsTimestampToUtcNow()
    {
        var before = DateTime.UtcNow;

        var movement = StockMovement.Create(Guid.NewGuid(), MovementType.Outbound, 10, null, "sales");

        var after = DateTime.UtcNow;

        movement.Timestamp.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }

    [Fact]
    public void Create_SetsCreatedByCorrectly()
    {
        var movement = StockMovement.Create(Guid.NewGuid(), MovementType.Inbound, 5, null, "admin");

        movement.CreatedBy.Should().Be("admin");
    }
}
