using ColdChainMonitor.Application.Interfaces;
using ColdChainMonitor.Domain.Entities;
using ColdChainMonitor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ColdChainMonitor.Infrastructure.Repositories;

public class DeviceRepository(ColdChainDbContext db) : IDeviceRepository
{
    public Task<Device?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        db.Devices.FirstOrDefaultAsync(d => d.Id == id, ct);

    public async Task<IReadOnlyList<Device>> GetActiveAsync(CancellationToken ct = default) =>
        await db.Devices.Where(d => d.IsActive).ToListAsync(ct);

    public async Task AddAsync(Device device, CancellationToken ct = default)
    {
        db.Devices.Add(device);
        await db.SaveChangesAsync(ct);
    }
}
