using ColdChainMonitor.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ColdChainMonitor.Infrastructure.Persistence;

public class ColdChainDbContext(DbContextOptions<ColdChainDbContext> options) : DbContext(options)
{
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<Reading> Readings => Set<Reading>();
    public DbSet<AlertRule> AlertRules => Set<AlertRule>();
    public DbSet<Excursion> Excursions => Set<Excursion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ColdChainDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
