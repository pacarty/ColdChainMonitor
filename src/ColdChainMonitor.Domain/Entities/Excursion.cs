using ColdChainMonitor.Domain.Enums;

namespace ColdChainMonitor.Domain.Entities;

public class Excursion
{
    public Guid Id { get; private set; }
    public Guid DeviceId { get; private set; }
    public Guid ReadingId { get; private set; }
    public ExcursionSeverity Severity { get; private set; }
    public string Message { get; private set; } = default!;
    public DateTime DetectedAtUtc { get; private set; }
    public DateTime? ResolvedAtUtc { get; private set; }

    private Excursion() { }

    public Excursion(Guid deviceId, Guid readingId, ExcursionSeverity severity, string message)
    {
        Id = Guid.NewGuid();
        DeviceId = deviceId;
        ReadingId = readingId;
        Severity = severity;
        Message = message;
        DetectedAtUtc = DateTime.UtcNow;
    }

    public void Resolve() => ResolvedAtUtc = DateTime.UtcNow;
}
