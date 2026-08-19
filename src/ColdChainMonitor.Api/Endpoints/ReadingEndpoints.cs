using ColdChainMonitor.Application.Dtos;
using ColdChainMonitor.Application.Interfaces;

namespace ColdChainMonitor.Api.Endpoints;

public static class ReadingEndpoints
{
    public static void MapReadingEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api").WithTags("Readings");

        // Ingestion: validate the device exists, then hand off to the queue.
        // Deliberately does NOT write to the database here — that's the Processor's job.
        group.MapPost("/readings", async (
            ReadingIngestDto reading,
            IDeviceRepository devices,
            IReadingQueuePublisher publisher,
            CancellationToken ct) =>
        {
            var device = await devices.GetByIdAsync(reading.DeviceId, ct);
            if (device is null)
            {
                return Results.NotFound($"Device {reading.DeviceId} not found.");
            }

            await publisher.PublishAsync(reading, ct);
            return Results.Accepted();
        });

        // Query: reads straight from Postgres — no queue involved on this path.
        group.MapGet("/devices/{deviceId:guid}/readings", async (
            Guid deviceId,
            int? hours,
            IReadingRepository readings,
            CancellationToken ct) =>
        {
            var windowHours = hours ?? 24;
            var since = DateTime.UtcNow.AddHours(-windowHours);
            var result = await readings.GetForDeviceAsync(deviceId, since, ct);

            var dtos = result.Select(r => new ReadingDto(
                r.Id, r.DeviceId, r.TemperatureCelsius, r.HumidityPercent, r.RecordedAtUtc));

            return Results.Ok(dtos);
        });
    }
}
