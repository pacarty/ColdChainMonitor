using ColdChainMonitor.Api.Endpoints;
using ColdChainMonitor.Infrastructure.DependencyInjection;
using ColdChainMonitor.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger stays available in every environment, including the deployed one — this is
// a portfolio project meant to be demoed live, not an internal production service.
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    // Convenience for local dev only: apply pending EF Core migrations on startup.
    // In Azure, migrations are applied manually and deliberately — not on every restart.
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ColdChainDbContext>();
    await db.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.MapDeviceEndpoints();
app.MapReadingEndpoints();
app.MapAlertRuleEndpoints();

app.Run();
