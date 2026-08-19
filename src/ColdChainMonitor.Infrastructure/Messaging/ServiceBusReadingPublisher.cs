using System.Text.Json;
using Azure.Messaging.ServiceBus;
using ColdChainMonitor.Application.Dtos;
using ColdChainMonitor.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace ColdChainMonitor.Infrastructure.Messaging;

public class ServiceBusReadingPublisher : IReadingQueuePublisher, IAsyncDisposable
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;

    public ServiceBusReadingPublisher(IOptions<ServiceBusOptions> options)
    {
        var settings = options.Value;
        _client = new ServiceBusClient(settings.ConnectionString);
        _sender = _client.CreateSender(settings.QueueName);
    }

    public async Task PublishAsync(ReadingIngestDto reading, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(reading);
        var message = new ServiceBusMessage(json)
        {
            ContentType = "application/json",
            Subject = "reading.ingested"
        };
        await _sender.SendMessageAsync(message, ct);
    }

    public async ValueTask DisposeAsync()
    {
        await _sender.DisposeAsync();
        await _client.DisposeAsync();
    }
}
