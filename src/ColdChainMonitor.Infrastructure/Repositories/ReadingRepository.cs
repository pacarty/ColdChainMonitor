using ColdChainMonitor.Application.Interfaces;
using ColdChainMonitor.Domain.Entities;
using ColdChainMonitor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ColdChainMonitor.Infrastructure.Repositories;

public class ReadingRepository(ColdChainDbContext db) : IReadingRepository
{
    public async Task AddAsync(Reading reading, CancellationToken ct = default)
    {
        db.Readings.Add(reading);
        await db.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<Reading>> GetForDeviceAsync(Guid deviceId, DateTime sinceUtc, CancellationToken ct = default) =>
        await db.Readings
            .Where(r => r.DeviceId == deviceId && r.RecordedAtUtc >= sinceUtc)
            .OrderBy(r => r.RecordedAtUtc)
            .ToListAsync(ct);
}
