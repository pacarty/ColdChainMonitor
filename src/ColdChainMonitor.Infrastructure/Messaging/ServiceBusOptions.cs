namespace ColdChainMonitor.Infrastructure.Messaging;

public class ServiceBusOptions
{
    public const string SectionName = "ServiceBus";

    public string ConnectionString { get; set; } = default!;
    public string QueueName { get; set; } = "readings";
}
