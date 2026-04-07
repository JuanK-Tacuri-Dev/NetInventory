using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetInventory.Domain.Entities;

namespace NetInventory.Infrastructure.Persistence.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("tbRefreshTokens");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Token).IsRequired().HasMaxLength(128);
        builder.Property(x => x.UserId).IsRequired();
        builder.HasIndex(x => x.Token);
        builder.HasIndex(x => x.UserId);
    }
}
