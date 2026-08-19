namespace ColdChainMonitor.Application.Dtos;

public record ReadingDto(
    Guid Id,
    Guid DeviceId,
    double TemperatureCelsius,
    double? HumidityPercent,
    DateTime RecordedAtUtc);
