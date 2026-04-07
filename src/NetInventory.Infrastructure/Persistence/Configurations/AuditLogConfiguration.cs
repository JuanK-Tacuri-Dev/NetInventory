using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetInventory.Domain.Entities;

namespace NetInventory.Infrastructure.Persistence.Configurations;

public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("tbAuditLogs");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CorrelationId).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Method).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Path).IsRequired().HasMaxLength(500);
        builder.Property(x => x.QueryString).HasMaxLength(1000);
        builder.Property(x => x.RequestBody).HasMaxLength(8000);
        builder.Property(x => x.ResponseBody).HasMaxLength(8000);
        builder.Property(x => x.StatusCode).IsRequired();
        builder.Property(x => x.DurationMs).IsRequired();
        builder.Property(x => x.UserId).HasMaxLength(450);
        builder.Property(x => x.UserEmail).HasMaxLength(256);
        builder.Property(x => x.OccurredAt).IsRequired();

        builder.HasIndex(x => x.CorrelationId);
        builder.HasIndex(x => x.OccurredAt);
    }
}
