using ColdChainMonitor.Domain.Entities;
using ColdChainMonitor.Domain.Enums;

namespace ColdChainMonitor.Application.Services;

public static class ThresholdEvaluator
{
    // How far outside the rule's range counts as "critical" rather than "warning".
    private const double CriticalMarginCelsius = 5.0;

    public static ExcursionSeverity? Evaluate(AlertRule rule, double temperatureCelsius)
    {
        if (!rule.IsBreachedBy(temperatureCelsius))
            return null;

        var distanceOutOfRange = temperatureCelsius < rule.MinTemperatureCelsius
            ? rule.MinTemperatureCelsius - temperatureCelsius
            : temperatureCelsius - rule.MaxTemperatureCelsius;

        return distanceOutOfRange >= CriticalMarginCelsius
            ? ExcursionSeverity.Critical
            : ExcursionSeverity.Warning;
    }
}
