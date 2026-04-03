using Microsoft.Extensions.Hosting;

namespace SportManagementSystem.Modules.Analytics.Infrastructure.ClickHouse;

public class ClickHouseAnalyticsInitializer(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<IClickHouseAnalyticsClient>();
        await client.EnsureSchemaAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
