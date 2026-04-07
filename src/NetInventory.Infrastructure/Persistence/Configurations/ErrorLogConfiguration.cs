using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetInventory.Domain.Entities;

namespace NetInventory.Infrastructure.Persistence.Configurations;

public sealed class ErrorLogConfiguration : IEntityTypeConfiguration<ErrorLog>
{
    public void Configure(EntityTypeBuilder<ErrorLog> builder)
    {
        builder.ToTable("tbErrorLogs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ReferenceCode).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => x.ReferenceCode).IsUnique();
        builder.Property(x => x.CorrelationId).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ExceptionType).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.StackTrace).HasMaxLength(8000);
        builder.Property(x => x.Path).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Method).HasMaxLength(10).IsRequired();
        builder.HasIndex(x => x.OccurredAt);
        builder.HasIndex(x => x.CorrelationId);
    }
}
