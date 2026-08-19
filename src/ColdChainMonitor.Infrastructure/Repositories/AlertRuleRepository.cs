using ColdChainMonitor.Application.Interfaces;
using ColdChainMonitor.Domain.Entities;
using ColdChainMonitor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ColdChainMonitor.Infrastructure.Repositories;

public class AlertRuleRepository(ColdChainDbContext db) : IAlertRuleRepository
{
    public async Task<IReadOnlyList<AlertRule>> GetApplicableRulesAsync(Guid deviceId, CancellationToken ct = default) =>
        await db.AlertRules
            .Where(r => r.IsActive && (r.DeviceId == deviceId || r.DeviceId == null))
            .ToListAsync(ct);

    public async Task AddAsync(AlertRule alertRule, CancellationToken ct = default)
    {
        db.AlertRules.Add(alertRule);
        await db.SaveChangesAsync(ct);
    }
}
