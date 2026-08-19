using ColdChainMonitor.Application.Interfaces;
using ColdChainMonitor.Domain.Entities;
using ColdChainMonitor.Infrastructure.Persistence;

namespace ColdChainMonitor.Infrastructure.Repositories;

public class ExcursionRepository(ColdChainDbContext db) : IExcursionRepository
{
    public async Task AddAsync(Excursion excursion, CancellationToken ct = default)
    {
        db.Excursions.Add(excursion);
        await db.SaveChangesAsync(ct);
    }
}
