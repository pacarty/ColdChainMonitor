using ColdChainMonitor.Domain.Entities;

namespace ColdChainMonitor.Application.Interfaces;

public interface IDeviceRepository
{
    Task<Device?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Device>> GetActiveAsync(CancellationToken ct = default);
    Task AddAsync(Device device, CancellationToken ct = default);
}
