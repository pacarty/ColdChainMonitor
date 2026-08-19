using System.Text.Json;
using Azure.Messaging.ServiceBus;
using ColdChainMonitor.Application.Dtos;
using ColdChainMonitor.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ColdChainMonitor.Infrastructure.Messaging;

public class ServiceBusReadingConsumer : IReadingQueueConsumer, IAsyncDisposable
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusProcessor _processor;
    private readonly ILogger<ServiceBusReadingConsumer> _logger;

    public ServiceBusReadingConsumer(IOptions<ServiceBusOptions> options, ILogger<ServiceBusReadingConsumer> logger)
    {
        _logger = logger;
        var settings = options.Value;
        _client = new ServiceBusClient(settings.ConnectionString);
        _processor = _client.CreateProcessor(settings.QueueName, new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 5,
            AutoCompleteMessages = false
        });
    }

    public async Task StartAsync(Func<ReadingIngestDto, CancellationToken, Task> onMessageReceived, CancellationToken ct)
    {
        _processor.ProcessMessageAsync += async args =>
        {
            try
            {
                var dto = JsonSerializer.Deserialize<ReadingIngestDto>(args.Message.Body.ToString());
                if (dto is not null)
                {
                    await onMessageReceived(dto, args.CancellationToken);
                }
                await args.CompleteMessageAsync(args.Message, args.CancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process reading message {MessageId}", args.Message.MessageId);
                await args.AbandonMessageAsync(args.Message, cancellationToken: args.CancellationToken);
            }
        };

        _processor.ProcessErrorAsync += args =>
        {
            _logger.LogError(args.Exception, "Service Bus processor error in {ErrorSource}", args.ErrorSource);
            return Task.CompletedTask;
        };

        await _processor.StartProcessingAsync(ct);
    }

    public async ValueTask DisposeAsync()
    {
        await _processor.DisposeAsync();
        await _client.DisposeAsync();
    }
}
