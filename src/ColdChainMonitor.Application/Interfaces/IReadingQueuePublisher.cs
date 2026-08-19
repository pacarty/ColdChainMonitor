using ColdChainMonitor.Application.Dtos;

namespace ColdChainMonitor.Application.Interfaces;

public interface IReadingQueuePublisher
{
    Task PublishAsync(ReadingIngestDto reading, CancellationToken ct = default);
}
