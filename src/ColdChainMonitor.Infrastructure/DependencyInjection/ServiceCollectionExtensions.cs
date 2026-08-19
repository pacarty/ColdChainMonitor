using ColdChainMonitor.Application.Interfaces;
using ColdChainMonitor.Application.Services;
using ColdChainMonitor.Infrastructure.Messaging;
using ColdChainMonitor.Infrastructure.Notifications;
using ColdChainMonitor.Infrastructure.Persistence;
using ColdChainMonitor.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ColdChainMonitor.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ColdChainDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));

        services.Configure<ServiceBusOptions>(configuration.GetSection(ServiceBusOptions.SectionName));

        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<IReadingRepository, ReadingRepository>();
        services.AddScoped<IAlertRuleRepository, AlertRuleRepository>();
        services.AddScoped<IExcursionRepository, ExcursionRepository>();

        // Singleton: these own a long-lived ServiceBusClient connection.
        services.AddSingleton<IReadingQueuePublisher, ServiceBusReadingPublisher>();
        services.AddSingleton<IReadingQueueConsumer, ServiceBusReadingConsumer>();

        services.AddScoped<IAlertNotifier, LoggingAlertNotifier>();

        // Orchestrator used by the Processor worker. Scoped because it depends on
        // scoped repositories (which depend on the scoped DbContext).
        services.AddScoped<ReadingProcessingService>();

        return services;
    }
}
