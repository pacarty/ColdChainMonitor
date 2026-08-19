using ColdChainMonitor.Application.Interfaces;
using ColdChainMonitor.Domain.Entities;

namespace ColdChainMonitor.Api.Endpoints;

public static class DeviceEndpoints
{
    public static void MapDeviceEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/devices").WithTags("Devices");

        group.MapPost("/", async (CreateDeviceRequest request, IDeviceRepository devices, CancellationToken ct) =>
        {
            var device = new Device(request.Name, request.Location);
            await devices.AddAsync(device, ct);
            return Results.Created($"/api/devices/{device.Id}", device);
        });

        group.MapGet("/", async (IDeviceRepository devices, CancellationToken ct) =>
        {
            var active = await devices.GetActiveAsync(ct);
            return Results.Ok(active);
        });
    }

    public record CreateDeviceRequest(string Name, string Location);
}
