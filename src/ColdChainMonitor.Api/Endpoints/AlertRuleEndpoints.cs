using ColdChainMonitor.Application.Interfaces;
using ColdChainMonitor.Domain.Entities;

namespace ColdChainMonitor.Api.Endpoints;

public static class AlertRuleEndpoints
{
    public static void MapAlertRuleEndpoints(this WebApplication app)
    {
        app.MapPost("/api/alert-rules", async (
            CreateAlertRuleRequest request,
            IDeviceRepository devices,
            IAlertRuleRepository alertRules,
            CancellationToken ct) =>
        {
            if (request.DeviceId is not null)
            {
                var device = await devices.GetByIdAsync(request.DeviceId.Value, ct);
                if (device is null)
                {
                    return Results.NotFound($"Device {request.DeviceId} not found.");
                }
            }

            try
            {
                var rule = new AlertRule(request.DeviceId, request.MinTemperatureCelsius, request.MaxTemperatureCelsius);
                await alertRules.AddAsync(rule, ct);
                return Results.Created($"/api/alert-rules/{rule.Id}", rule);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }).WithTags("AlertRules");

        app.MapGet("/api/devices/{deviceId:guid}/alert-rules", async (
            Guid deviceId,
            IAlertRuleRepository alertRules,
            CancellationToken ct) =>
        {
            var rules = await alertRules.GetApplicableRulesAsync(deviceId, ct);
            return Results.Ok(rules);
        }).WithTags("AlertRules");
    }

    public record CreateAlertRuleRequest(Guid? DeviceId, double MinTemperatureCelsius, double MaxTemperatureCelsius);
}
