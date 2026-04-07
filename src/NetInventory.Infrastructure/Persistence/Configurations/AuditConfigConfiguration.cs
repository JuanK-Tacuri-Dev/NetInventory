using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetInventory.Domain.Entities;

namespace NetInventory.Infrastructure.Persistence.Configurations;

public sealed class AuditConfigConfiguration : IEntityTypeConfiguration<AuditConfig>
{
    public void Configure(EntityTypeBuilder<AuditConfig> builder)
    {
        builder.ToTable("tbAuditConfigs");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Method).IsRequired().HasMaxLength(10);
        builder.Property(x => x.UrlPattern).IsRequired().HasMaxLength(500);
        builder.Property(x => x.IsEnabled).IsRequired();
        builder.Property(x => x.Description).IsRequired().HasMaxLength(200);
        builder.Property(x => x.CreatedAt).IsRequired();
    }
}
