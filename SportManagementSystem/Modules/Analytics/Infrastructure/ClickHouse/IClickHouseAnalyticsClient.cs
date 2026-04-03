namespace SportManagementSystem.Modules.Analytics.Infrastructure.ClickHouse;

public interface IClickHouseAnalyticsClient
{
    Task EnsureSchemaAsync(CancellationToken cancellationToken);
}
