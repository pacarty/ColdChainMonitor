using ColdChainMonitor.Application.Dtos;

namespace ColdChainMonitor.Application.Interfaces;

public interface IReadingQueueConsumer
{
    // Starts listening and invokes the handler for each message received.
    // Infrastructure's Service Bus implementation owns the receive loop; Application just supplies the callback.
    Task StartAsync(Func<ReadingIngestDto, CancellationToken, Task> onMessageReceived, CancellationToken ct);
}
