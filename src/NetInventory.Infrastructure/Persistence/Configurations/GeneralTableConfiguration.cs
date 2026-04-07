using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetInventory.Domain.Entities;

namespace NetInventory.Infrastructure.Persistence.Configurations;

public sealed class GeneralTableConfiguration : IEntityTypeConfiguration<GeneralTable>
{
    public void Configure(EntityTypeBuilder<GeneralTable> builder)
    {
        builder.ToTable("tbGeneralTable");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Description).IsRequired().HasMaxLength(200);
        builder.Property(x => x.IsActive).IsRequired();
    }
}
