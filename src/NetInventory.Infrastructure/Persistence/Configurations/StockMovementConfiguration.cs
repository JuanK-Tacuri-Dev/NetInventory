using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetInventory.Domain.Entities;

namespace NetInventory.Infrastructure.Persistence.Configurations;

public sealed class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("tbStockMovements");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.ProductId)
            .IsRequired();

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(m => m.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(m => m.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(m => m.Quantity)
            .IsRequired();

        builder.Property(m => m.Reason)
            .HasMaxLength(500);

        builder.Property(m => m.Timestamp);
        builder.Property(m => m.CreatedBy);
    }
}
