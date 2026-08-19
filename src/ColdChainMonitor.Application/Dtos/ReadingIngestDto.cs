namespace ColdChainMonitor.Application.Dtos;

public record ReadingIngestDto(
    Guid DeviceId,
    double TemperatureCelsius,
    double? HumidityPercent,
    DateTime RecordedAtUtc);
