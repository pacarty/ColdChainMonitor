using System.Net.Http.Json;
using Microsoft.Extensions.Options;

namespace ColdChainMonitor.DeviceSimulator;

public class DeviceSimulationWorker(
    IHttpClientFactory httpClientFactory,
    IOptions<SimulatorOptions> options,
    ILogger<DeviceSimulationWorker> logger) : BackgroundService
{
    private readonly SimulatorOptions _options = options.Value;
    private readonly Random _random = new();
    private readonly List<SimulatedDevice> _devices = [];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = httpClientFactory.CreateClient("ColdChainApi");

        await RegisterSimulatedDevicesAsync(client, stoppingToken);

        if (_devices.Count == 0)
        {
            logger.LogError(
                "No devices registered — simulator has nothing to do. Is the Api running at {ApiBaseUrl}?",
                _options.ApiBaseUrl);
            return;
        }

        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(_options.IntervalSeconds));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            foreach (var device in _devices)
            {
                await SendReadingAsync(client, device, stoppingToken);
            }
        }
    }

    private async Task RegisterSimulatedDevicesAsync(HttpClient client, CancellationToken ct)
    {
        // Registers a fresh batch of devices every run rather than reusing ones from a
        // previous session. Fine for a demo; a "load existing device IDs from config"
        // mode would be the natural next step if this ran unattended for longer periods.
        for (var i = 1; i <= _options.DeviceCount; i++)
        {
            var request = new CreateDeviceRequest($"Simulated Freezer {i}", $"Simulated Warehouse {((i - 1) / 2) + 1}");
            var response = await client.PostAsJsonAsync("/api/devices", request, ct);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Failed to register simulated device {Index}: {StatusCode}", i, response.StatusCode);
                continue;
            }

            var created = await response.Content.ReadFromJsonAsync<CreatedDeviceResponse>(cancellationToken: ct);
            if (created is null) continue;

            // Baseline sits somewhere in a typical frozen-goods range; each device drifts
            // around its own baseline so the fleet doesn't all report identical numbers.
            var baseline = -20 + _random.NextDouble() * 2; // roughly -20 to -18°C
            _devices.Add(new SimulatedDevice(created.Id, created.Name, baseline));
            logger.LogInformation("Registered {Name} ({Id}), baseline {Baseline:F1}°C", created.Name, created.Id, baseline);
        }
    }

    private async Task SendReadingAsync(HttpClient client, SimulatedDevice device, CancellationToken ct)
    {
        var isExcursion = _random.NextDouble() < _options.ExcursionProbability;

        var temperature = isExcursion
            ? device.BaselineTemperatureCelsius + 15 + _random.NextDouble() * 10 // well outside range
            : device.BaselineTemperatureCelsius + (_random.NextDouble() * 1.0 - 0.5); // small normal jitter

        var reading = new ReadingIngestRequest(device.Id, Math.Round(temperature, 2), null, DateTime.UtcNow);

        var response = await client.PostAsJsonAsync("/api/readings", reading, ct);

        if (response.IsSuccessStatusCode)
        {
            logger.LogInformation("{Name}: {Temp:F1}°C{Flag}", device.Name, temperature, isExcursion ? " (excursion)" : "");
        }
        else
        {
            logger.LogWarning("{Name}: failed to send reading — {StatusCode}", device.Name, response.StatusCode);
        }
    }
}

public record SimulatedDevice(Guid Id, string Name, double BaselineTemperatureCelsius);
public record CreateDeviceRequest(string Name, string Location);
public record CreatedDeviceResponse(Guid Id, string Name, string Location);
public record ReadingIngestRequest(Guid DeviceId, double TemperatureCelsius, double? HumidityPercent, DateTime RecordedAtUtc);
