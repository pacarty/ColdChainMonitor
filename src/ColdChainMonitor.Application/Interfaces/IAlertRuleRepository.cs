using ColdChainMonitor.Domain.Entities;

namespace ColdChainMonitor.Application.Interfaces;

public interface IAlertRuleRepository
{
    Task<IReadOnlyList<AlertRule>> GetApplicableRulesAsync(Guid deviceId, CancellationToken ct = default);
    Task AddAsync(AlertRule alertRule, CancellationToken ct = default);
}
