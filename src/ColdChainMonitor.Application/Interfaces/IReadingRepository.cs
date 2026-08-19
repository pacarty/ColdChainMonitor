using ColdChainMonitor.Domain.Entities;

namespace ColdChainMonitor.Application.Interfaces;

public interface IReadingRepository
{
    Task AddAsync(Reading reading, CancellationToken ct = default);
    Task<IReadOnlyList<Reading>> GetForDeviceAsync(Guid deviceId, DateTime sinceUtc, CancellationToken ct = default);
}
