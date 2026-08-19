using ColdChainMonitor.Application.Dtos;
using ColdChainMonitor.Application.Interfaces;
using ColdChainMonitor.Domain.Entities;

namespace ColdChainMonitor.Application.Services;

public class ReadingProcessingService(
    IReadingRepository readings,
    IAlertRuleRepository alertRules,
    IExcursionRepository excursions,
    IAlertNotifier notifier)
{
    public async Task ProcessAsync(ReadingIngestDto dto, CancellationToken ct = default)
    {
        var reading = new Reading(dto.DeviceId, dto.TemperatureCelsius, dto.HumidityPercent, dto.RecordedAtUtc);
        await readings.AddAsync(reading, ct);

        var rules = await alertRules.GetApplicableRulesAsync(dto.DeviceId, ct);

        foreach (var rule in rules)
        {
            var severity = ThresholdEvaluator.Evaluate(rule, dto.TemperatureCelsius);
            if (severity is null) continue;

            var excursion = new Excursion(
                dto.DeviceId,
                reading.Id,
                severity.Value,
                $"Reading {dto.TemperatureCelsius:F1}\u00b0C outside range [{rule.MinTemperatureCelsius:F1}, {rule.MaxTemperatureCelsius:F1}]\u00b0C");

            await excursions.AddAsync(excursion, ct);
            await notifier.NotifyAsync(excursion, ct);
        }
    }
}
