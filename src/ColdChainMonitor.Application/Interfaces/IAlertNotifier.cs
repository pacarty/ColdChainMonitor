using ColdChainMonitor.Domain.Entities;

namespace ColdChainMonitor.Application.Interfaces;

public interface IAlertNotifier
{
    Task NotifyAsync(Excursion excursion, CancellationToken ct = default);
}
