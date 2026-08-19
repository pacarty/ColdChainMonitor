namespace ColdChainMonitor.Application.Dtos;

public record DeviceStatsDto(
    Guid DeviceId,
    double MinTemperatureCelsius,
    double MaxTemperatureCelsius,
    double AverageTemperatureCelsius,
    int ReadingCount,
    DateTime WindowStartUtc,
    DateTime WindowEndUtc);
