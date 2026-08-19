using ColdChainMonitor.Application.Interfaces;
using ColdChainMonitor.Application.Services;

namespace ColdChainMonitor.Processor;

public class ReadingConsumerWorker(
    IReadingQueueConsumer consumer,
    IServiceScopeFactory scopeFactory,
    ILogger<ReadingConsumerWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting reading consumer...");

        // StartProcessingAsync on the underlying Service Bus processor returns immediately
        // once the receive loop is armed — messages arrive via the event handler, not here.
        await consumer.StartAsync(async (dto, ct) =>
        {
            // Each message gets its own DI scope, since ReadingProcessingService depends on
            // scoped repositories backed by a scoped DbContext — reusing one scope across
            // messages would risk stale/shared DbContext state under concurrent processing.
            using var scope = scopeFactory.CreateScope();
            var processor = scope.ServiceProvider.GetRequiredService<ReadingProcessingService>();
            await processor.ProcessAsync(dto, ct);
        }, stoppingToken);

        logger.LogInformation("Reading consumer started. Waiting for messages...");

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Reading consumer stopping.");
        }
    }
}
