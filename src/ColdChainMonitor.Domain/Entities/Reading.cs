namespace ColdChainMonitor.Domain.Entities;

public class Reading
{
    public Guid Id { get; private set; }
    public Guid DeviceId { get; private set; }
    public double TemperatureCelsius { get; private set; }
    public double? HumidityPercent { get; private set; }
    public DateTime RecordedAtUtc { get; private set; }

    private Reading() { }

    public Reading(Guid deviceId, double temperatureCelsius, double? humidityPercent, DateTime recordedAtUtc)
    {
        Id = Guid.NewGuid();
        DeviceId = deviceId;
        TemperatureCelsius = temperatureCelsius;
        HumidityPercent = humidityPercent;
        RecordedAtUtc = recordedAtUtc;
    }
}
