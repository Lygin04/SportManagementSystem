using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;
using SportManagementSystem.Modules.Analytics.Application.EventHandlers;
using SportManagementSystem.Modules.Analytics.Application.Repositories;
using SportManagementSystem.Modules.Analytics.Infrastructure.ClickHouse;

namespace SportManagementSystem.Extensions;

public static class ClickHouseAnalyticsExtensions
{
    public static IServiceCollection AddClickHouseAnalytics(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ClickHouseOptions>(configuration.GetSection("ClickHouse"));

        services.AddHttpClient<ClickHouseAnalyticsClient>((provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<ClickHouseOptions>>().Value;
            client.BaseAddress = new Uri(options.HttpUrl.TrimEnd('/') + "/");

            if (string.IsNullOrWhiteSpace(options.Username))
            {
                return;
            }

            var authBytes = Encoding.UTF8.GetBytes($"{options.Username}:{options.Password}");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));
        });

        services.AddScoped<IClickHouseAnalyticsClient>(provider =>
            provider.GetRequiredService<ClickHouseAnalyticsClient>());
        services.AddScoped<IClientAnalyticsRepository>(provider =>
            provider.GetRequiredService<ClickHouseAnalyticsClient>());
        services.AddScoped<IManagerAnalyticsRepository>(provider =>
            provider.GetRequiredService<ClickHouseAnalyticsClient>());
        services.AddScoped<IAnalyticsEventWriter>(provider =>
            provider.GetRequiredService<ClickHouseAnalyticsClient>());
        services.AddHostedService<ClickHouseAnalyticsInitializer>();

        return services;
    }
}
