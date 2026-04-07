using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetInventory.Domain.Entities;

namespace NetInventory.Infrastructure.Persistence.Configurations;

public sealed class GeneralValueConfiguration : IEntityTypeConfiguration<GeneralValue>
{
    public void Configure(EntityTypeBuilder<GeneralValue> builder)
    {
        builder.ToTable("tbGeneralValues");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(20);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Value).HasMaxLength(500);
        builder.Property(x => x.SortOrder).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();

        builder.HasIndex(x => new { x.TableId, x.Code }).IsUnique();

        builder.HasOne<GeneralTable>()
            .WithMany()
            .HasForeignKey(x => x.TableId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<GeneralValue>()
            .WithMany()
            .HasForeignKey(x => x.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
