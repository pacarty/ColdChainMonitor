using ColdChainMonitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ColdChainMonitor.Infrastructure.Persistence.Configurations;

public class ExcursionConfiguration : IEntityTypeConfiguration<Excursion>
{
    public void Configure(EntityTypeBuilder<Excursion> builder)
    {
        builder.ToTable("Excursions");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.DeviceId);
        builder.Property(e => e.Message).IsRequired().HasMaxLength(500);
    }
}
