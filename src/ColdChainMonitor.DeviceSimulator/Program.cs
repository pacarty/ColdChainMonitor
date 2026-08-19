using ColdChainMonitor.DeviceSimulator;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<SimulatorOptions>(builder.Configuration.GetSection(SimulatorOptions.SectionName));

builder.Services.AddHttpClient("ColdChainApi", (sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<SimulatorOptions>>().Value;
    client.BaseAddress = new Uri(options.ApiBaseUrl);
});

builder.Services.AddHostedService<DeviceSimulationWorker>();

var host = builder.Build();
host.Run();
