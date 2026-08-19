namespace ColdChainMonitor.Domain.Entities;

public class AlertRule
{
    public Guid Id { get; private set; }
    public Guid? DeviceId { get; private set; } // null = applies to all devices
    public double MinTemperatureCelsius { get; private set; }
    public double MaxTemperatureCelsius { get; private set; }
    public bool IsActive { get; private set; }

    private AlertRule() { }

    public AlertRule(Guid? deviceId, double minTemperatureCelsius, double maxTemperatureCelsius)
    {
        if (minTemperatureCelsius >= maxTemperatureCelsius)
            throw new ArgumentException("Min temperature must be less than max temperature.");

        Id = Guid.NewGuid();
        DeviceId = deviceId;
        MinTemperatureCelsius = minTemperatureCelsius;
        MaxTemperatureCelsius = maxTemperatureCelsius;
        IsActive = true;
    }

    public bool IsBreachedBy(double temperatureCelsius) =>
        temperatureCelsius < MinTemperatureCelsius || temperatureCelsius > MaxTemperatureCelsius;
}
