using ColdChainMonitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColdChainMonitor.Infrastructure.Persistence.Configurations;

public class ReadingConfiguration : IEntityTypeConfiguration<Reading>
{
    public void Configure(EntityTypeBuilder<Reading> builder)
    {
        builder.ToTable("Readings");
        builder.HasKey(r => r.Id);
        builder.HasIndex(r => new { r.DeviceId, r.RecordedAtUtc });
    }
}
