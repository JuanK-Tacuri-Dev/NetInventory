using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetInventory.Domain.Entities;

namespace NetInventory.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("tbProducts");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.OwnsOne(p => p.SKU, skuBuilder =>
        {
            skuBuilder.Property(s => s.Value)
                .HasColumnName("SKU")
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.Property(p => p.CategoryTableId).IsRequired();
        builder.Property(p => p.CategoryCode).IsRequired().HasMaxLength(20);

        builder.Property(p => p.QuantityInStock)
            .IsRequired();

        builder.Property(p => p.MinStock)
            .IsRequired()
            .HasDefaultValue(0)
            .ValueGeneratedNever();

        builder.Property(p => p.MaxStock)
            .IsRequired()
            .HasDefaultValue(0)
            .ValueGeneratedNever();

        builder.OwnsOne(p => p.UnitPrice, moneyBuilder =>
        {
            moneyBuilder.Property(m => m.Amount)
                .HasColumnName("UnitPrice")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });

        builder.Property(p => p.CreatedAt);
        builder.Property(p => p.CreatedBy);
        builder.Property(p => p.UpdatedBy);

        builder.Property(p => p.OwnerId)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasIndex(p => p.OwnerId);
    }
}
