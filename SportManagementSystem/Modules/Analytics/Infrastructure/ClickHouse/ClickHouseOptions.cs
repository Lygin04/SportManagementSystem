namespace SportManagementSystem.Modules.Analytics.Infrastructure.ClickHouse;

public sealed class ClickHouseOptions
{
    public string HttpUrl { get; set; } = "http://clickhouse:8123";
    public string Database { get; set; } = "sport_management_analytics";
    public string Username { get; set; } = "clickhouse";
    public string Password { get; set; } = "password";
}
