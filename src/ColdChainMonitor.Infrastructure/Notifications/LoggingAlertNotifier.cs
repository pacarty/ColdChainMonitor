using ColdChainMonitor.Application.Interfaces;
using ColdChainMonitor.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ColdChainMonitor.Infrastructure.Notifications;

// Placeholder notifier — logs the excursion. Swap in a SendGrid or webhook
// implementation later behind the same IAlertNotifier interface.
public class LoggingAlertNotifier(ILogger<LoggingAlertNotifier> logger) : IAlertNotifier
{
    public Task NotifyAsync(Excursion excursion, CancellationToken ct = default)
    {
        logger.LogWarning(
            "Excursion detected: Device {DeviceId}, Severity {Severity}, {Message}",
            excursion.DeviceId, excursion.Severity, excursion.Message);
        return Task.CompletedTask;
    }
}
