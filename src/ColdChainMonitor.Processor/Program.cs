using ColdChainMonitor.Infrastructure.DependencyInjection;
using ColdChainMonitor.Processor;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHostedService<ReadingConsumerWorker>();

var host = builder.Build();
host.Run();
