using System.Globalization;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SportManagementSystem.Modules.Analytics.Application.EventHandlers;
using SportManagementSystem.Modules.Analytics.Application.Queries.GetClientAnalytics;
using SportManagementSystem.Modules.Analytics.Application.Queries.GetManagerBranchAnalytics;
using SportManagementSystem.Modules.Analytics.Application.Repositories;

namespace SportManagementSystem.Modules.Analytics.Infrastructure.ClickHouse;

public class ClickHouseAnalyticsClient(HttpClient httpClient, IOptions<ClickHouseOptions> options)
    : IClickHouseAnalyticsClient, IClientAnalyticsRepository, IManagerAnalyticsRepository, IAnalyticsEventWriter
{
    private const int TrendDayWindow = 14;
    private readonly ClickHouseOptions _options = options.Value;

    public async Task EnsureSchemaAsync(CancellationToken cancellationToken)
    {
        await ExecuteCommandAsync(
            $"CREATE DATABASE IF NOT EXISTS {EscapeIdentifier(_options.Database)}",
            cancellationToken,
            includeDatabase: false);
        await ExecuteCommandAsync(
            $"""
             CREATE TABLE IF NOT EXISTS {EscapeIdentifier(_options.Database)}.client_activity_events
             (
                 event_id UUID,
                 occurred_on DateTime64(3, 'UTC'),
                 occurred_date Date DEFAULT toDate(occurred_on),
                 occurred_time String DEFAULT formatDateTime(occurred_on, '%H:%i:%S'),
                 occurred_hour UInt8 DEFAULT toHour(occurred_on),
                 occurred_minute UInt8 DEFAULT toMinute(occurred_on),
                 event_type LowCardinality(String),
                 client_id Int64,
                 source_entity_id Int64,
                 sport_service_id Int64,
                 branch_id Int64,
                 metadata_json String
             )
             ENGINE = MergeTree
             ORDER BY (client_id, event_type, occurred_date, occurred_hour, source_entity_id)
             """,
            cancellationToken);

        await ExecuteCommandAsync(
            $"""
             ALTER TABLE {EscapeIdentifier(_options.Database)}.client_activity_events
             ADD COLUMN IF NOT EXISTS occurred_date Date DEFAULT toDate(occurred_on)
             """,
            cancellationToken);
        await ExecuteCommandAsync(
            $"""
             ALTER TABLE {EscapeIdentifier(_options.Database)}.client_activity_events
             ADD COLUMN IF NOT EXISTS occurred_time String DEFAULT formatDateTime(occurred_on, '%H:%i:%S')
             """,
            cancellationToken);
        await ExecuteCommandAsync(
            $"""
             ALTER TABLE {EscapeIdentifier(_options.Database)}.client_activity_events
             ADD COLUMN IF NOT EXISTS occurred_hour UInt8 DEFAULT toHour(occurred_on)
             """,
            cancellationToken);
        await ExecuteCommandAsync(
            $"""
             ALTER TABLE {EscapeIdentifier(_options.Database)}.client_activity_events
             ADD COLUMN IF NOT EXISTS occurred_minute UInt8 DEFAULT toMinute(occurred_on)
             """,
            cancellationToken);
    }

    public Task WriteAsync(AnalyticsEventRecord analyticsEvent, CancellationToken cancellationToken)
    {
        var occurredAtUtc = analyticsEvent.OccurredOnUtc.UtcDateTime;
        var occurredDate = DateOnly.FromDateTime(occurredAtUtc);
        var occurredTime = occurredAtUtc.ToString("HH:mm:ss", CultureInfo.InvariantCulture);

        var sql =
            $"""
             INSERT INTO {EscapeIdentifier(_options.Database)}.client_activity_events
             (
                 event_id,
                 occurred_on,
                 occurred_date,
                 occurred_time,
                 occurred_hour,
                 occurred_minute,
                 event_type,
                 client_id,
                 source_entity_id,
                 sport_service_id,
                 branch_id,
                 metadata_json
             )
             VALUES
             (
                 '{analyticsEvent.EventId:D}',
                 toDateTime64('{occurredAtUtc.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}', 3, 'UTC'),
                 toDate('{occurredDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}'),
                 '{occurredTime}',
                 {occurredAtUtc.Hour},
                 {occurredAtUtc.Minute},
                 '{EscapeValue(analyticsEvent.EventType)}',
                 {analyticsEvent.ClientId},
                 {analyticsEvent.SourceEntityId},
                 {analyticsEvent.SportServiceId},
                 {analyticsEvent.BranchId},
                 '{EscapeValue(analyticsEvent.MetadataJson)}'
             )
             """;

        return ExecuteCommandAsync(sql, cancellationToken);
    }

    public async Task<ClientAnalyticsOverviewResponse> GetClientOverviewAsync(long clientId, CancellationToken cancellationToken)
    {
        var sql =
            $"""
             SELECT
                 {clientId} AS ClientId,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.TrainingSessionBooked}')) AS TrainingSessionBookings,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.MembershipPurchased}')) AS MembershipPurchases
             FROM {EscapeIdentifier(_options.Database)}.client_activity_events
             WHERE client_id = {clientId}
             FORMAT JSONEachRow
             """;

        using var response = await httpClient.PostAsync(BuildQueryUri(includeDatabase: true), CreateSqlContent(sql), cancellationToken);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(payload))
        {
            return new ClientAnalyticsOverviewResponse { ClientId = clientId };
        }

        var analytics = JsonSerializer.Deserialize<ClientAnalyticsOverviewResponse>(payload.Trim());
        return analytics ?? new ClientAnalyticsOverviewResponse { ClientId = clientId };
    }

    public async Task<ManagerBranchAnalyticsOverviewResponse> GetManagerOverviewAsync(
        IReadOnlyCollection<long> branchIds,
        string trendGranularity,
        DateOnly? selectedDate,
        long? selectedBranchId,
        CancellationToken cancellationToken)
    {
        var normalizedGranularity = AnalyticsTrendGranularities.Normalize(trendGranularity);
        if (branchIds.Count == 0)
        {
            return new ManagerBranchAnalyticsOverviewResponse
            {
                TrendGranularity = normalizedGranularity,
                SelectedBranchId = selectedBranchId
            };
        }

        var summaryInClause = string.Join(", ", branchIds);
        var trendBranchIds = selectedBranchId.HasValue ? [selectedBranchId.Value] : branchIds;
        var trendInClause = string.Join(", ", trendBranchIds);
        var summariesSql =
            $"""
             SELECT
                 branch_id AS BranchId,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.BranchCardViewed}')) AS BranchCardViews,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.BranchDetailsOpened}')) AS BranchDetailsOpens,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.TrainingSessionBooked}')) AS TrainingSessionBookings,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.MembershipPurchased}')) AS MembershipPurchases
             FROM {EscapeIdentifier(_options.Database)}.client_activity_events
             WHERE branch_id IN ({summaryInClause})
             GROUP BY branch_id
             FORMAT JSONEachRow
             """;

        var summaries = await ExecuteRowsQueryAsync<BranchAnalyticsSummaryResponse>(summariesSql, cancellationToken);
        var availableDates = await GetAvailableDatesAsync(trendInClause, cancellationToken);
        var effectiveSelectedDate = ResolveSelectedDate(selectedDate, availableDates);
        var dailyTrendEndDate = TryParseDate(availableDates.FirstOrDefault()) ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var trend = normalizedGranularity switch
        {
            AnalyticsTrendGranularities.Hour => await GetHourlyTrendAsync(trendInClause, effectiveSelectedDate, cancellationToken),
            AnalyticsTrendGranularities.TenMinutes => await GetTenMinuteTrendAsync(trendInClause, effectiveSelectedDate, cancellationToken),
            _ => await GetDailyTrendAsync(trendInClause, dailyTrendEndDate, cancellationToken)
        };

        return new ManagerBranchAnalyticsOverviewResponse
        {
            Branches = summaries,
            Trend = trend,
            TrendGranularity = normalizedGranularity,
            SelectedDate = effectiveSelectedDate,
            SelectedBranchId = selectedBranchId,
            AvailableDates = availableDates
        };
    }

    private async Task<List<string>> GetAvailableDatesAsync(string inClause, CancellationToken cancellationToken)
    {
        var sql =
            $"""
             SELECT
                 toString(occurred_date) AS Date
             FROM {EscapeIdentifier(_options.Database)}.client_activity_events
             WHERE branch_id IN ({inClause})
             GROUP BY occurred_date
             ORDER BY occurred_date DESC
             LIMIT {TrendDayWindow}
             FORMAT JSONEachRow
             """;

        var rows = await ExecuteRowsQueryAsync<DateRow>(sql, cancellationToken);
        return rows.Select(row => row.Date).ToList();
    }

    private async Task<List<BranchAnalyticsTrendPointResponse>> GetDailyTrendAsync(
        string inClause,
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        var startDate = endDate.AddDays(-(TrendDayWindow - 1));
        var sql =
            $"""
             SELECT
                 toString(occurred_date) AS Date,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.BranchCardViewed}')) AS BranchCardViews,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.BranchDetailsOpened}')) AS BranchDetailsOpens,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.TrainingSessionBooked}')) AS TrainingSessionBookings,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.MembershipPurchased}')) AS MembershipPurchases
             FROM {EscapeIdentifier(_options.Database)}.client_activity_events
             WHERE branch_id IN ({inClause})
               AND occurred_date >= toDate('{startDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}')
             GROUP BY occurred_date
             ORDER BY occurred_date
             FORMAT JSONEachRow
             """;

        var rows = await ExecuteRowsQueryAsync<DailyTrendRow>(sql, cancellationToken);
        var rowsByDate = rows.ToDictionary(row => row.Date, StringComparer.Ordinal);
        var trend = new List<BranchAnalyticsTrendPointResponse>(TrendDayWindow);

        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var key = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (rowsByDate.TryGetValue(key, out var row))
            {
                trend.Add(new BranchAnalyticsTrendPointResponse
                {
                    Date = key,
                    Time = string.Empty,
                    Label = date.ToString("dd.MM", CultureInfo.InvariantCulture),
                    BranchCardViews = row.BranchCardViews,
                    BranchDetailsOpens = row.BranchDetailsOpens,
                    TrainingSessionBookings = row.TrainingSessionBookings,
                    MembershipPurchases = row.MembershipPurchases
                });
                continue;
            }

            trend.Add(new BranchAnalyticsTrendPointResponse
            {
                Date = key,
                Time = string.Empty,
                Label = date.ToString("dd.MM", CultureInfo.InvariantCulture)
            });
        }

        return trend;
    }

    private async Task<List<BranchAnalyticsTrendPointResponse>> GetHourlyTrendAsync(
        string inClause,
        string? selectedDate,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(selectedDate))
        {
            return [];
        }

        var sql =
            $"""
             SELECT
                 occurred_hour AS Hour,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.BranchCardViewed}')) AS BranchCardViews,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.BranchDetailsOpened}')) AS BranchDetailsOpens,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.TrainingSessionBooked}')) AS TrainingSessionBookings,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.MembershipPurchased}')) AS MembershipPurchases
             FROM {EscapeIdentifier(_options.Database)}.client_activity_events
             WHERE branch_id IN ({inClause})
               AND occurred_date = toDate('{selectedDate}')
             GROUP BY occurred_hour
             ORDER BY occurred_hour
             FORMAT JSONEachRow
             """;

        var rows = await ExecuteRowsQueryAsync<HourlyTrendRow>(sql, cancellationToken);
        var rowsByHour = rows.ToDictionary(row => row.Hour);
        var trend = new List<BranchAnalyticsTrendPointResponse>(24);

        for (var hour = 0; hour < 24; hour++)
        {
            var timeLabel = $"{hour:00}:00";
            if (rowsByHour.TryGetValue(hour, out var row))
            {
                trend.Add(new BranchAnalyticsTrendPointResponse
                {
                    Date = selectedDate,
                    Time = timeLabel,
                    Label = timeLabel,
                    BranchCardViews = row.BranchCardViews,
                    BranchDetailsOpens = row.BranchDetailsOpens,
                    TrainingSessionBookings = row.TrainingSessionBookings,
                    MembershipPurchases = row.MembershipPurchases
                });
                continue;
            }

            trend.Add(new BranchAnalyticsTrendPointResponse
            {
                Date = selectedDate,
                Time = timeLabel,
                Label = timeLabel
            });
        }

        return trend;
    }

    private async Task<List<BranchAnalyticsTrendPointResponse>> GetTenMinuteTrendAsync(
        string inClause,
        string? selectedDate,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(selectedDate))
        {
            return [];
        }

        var sql =
            $"""
             SELECT
                 occurred_hour AS Hour,
                 intDiv(occurred_minute, 10) * 10 AS MinuteBucket,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.BranchCardViewed}')) AS BranchCardViews,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.BranchDetailsOpened}')) AS BranchDetailsOpens,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.TrainingSessionBooked}')) AS TrainingSessionBookings,
                 toInt32(countIf(event_type = '{AnalyticsEventTypes.MembershipPurchased}')) AS MembershipPurchases
             FROM {EscapeIdentifier(_options.Database)}.client_activity_events
             WHERE branch_id IN ({inClause})
               AND occurred_date = toDate('{selectedDate}')
             GROUP BY occurred_hour, MinuteBucket
             ORDER BY occurred_hour, MinuteBucket
             FORMAT JSONEachRow
             """;

        var rows = await ExecuteRowsQueryAsync<TenMinuteTrendRow>(sql, cancellationToken);
        var rowsByBucket = rows.ToDictionary(row => (row.Hour, row.MinuteBucket));
        var trend = new List<BranchAnalyticsTrendPointResponse>(24 * 6);

        for (var hour = 0; hour < 24; hour++)
        {
            for (var minuteBucket = 0; minuteBucket < 60; minuteBucket += 10)
            {
                var label = $"{hour:00}:{minuteBucket:00}";
                if (rowsByBucket.TryGetValue((hour, minuteBucket), out var row))
                {
                    trend.Add(new BranchAnalyticsTrendPointResponse
                    {
                        Date = selectedDate,
                        Time = label,
                        Label = label,
                        BranchCardViews = row.BranchCardViews,
                        BranchDetailsOpens = row.BranchDetailsOpens,
                        TrainingSessionBookings = row.TrainingSessionBookings,
                        MembershipPurchases = row.MembershipPurchases
                    });
                    continue;
                }

                trend.Add(new BranchAnalyticsTrendPointResponse
                {
                    Date = selectedDate,
                    Time = label,
                    Label = label
                });
            }
        }

        return trend;
    }

    private static string? ResolveSelectedDate(DateOnly? selectedDate, IReadOnlyCollection<string> availableDates)
    {
        if (availableDates.Count == 0)
        {
            return null;
        }

        if (selectedDate.HasValue)
        {
            var requested = selectedDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (availableDates.Contains(requested, StringComparer.Ordinal))
            {
                return requested;
            }
        }

        return availableDates.First();
    }

    private static DateOnly? TryParseDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return DateOnly.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate)
            ? parsedDate
            : null;
    }

    private async Task ExecuteCommandAsync(string sql, CancellationToken cancellationToken, bool includeDatabase = true)
    {
        using var response = await httpClient.PostAsync(BuildQueryUri(includeDatabase), CreateSqlContent(sql), cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    private string BuildQueryUri(bool includeDatabase)
    {
        var uri = "?wait_end_of_query=1";
        if (includeDatabase)
        {
            uri += $"&database={Uri.EscapeDataString(_options.Database)}";
        }

        return uri;
    }

    private async Task<List<T>> ExecuteRowsQueryAsync<T>(string sql, CancellationToken cancellationToken)
    {
        using var response = await httpClient.PostAsync(BuildQueryUri(includeDatabase: true), CreateSqlContent(sql), cancellationToken);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(payload))
        {
            return [];
        }

        return payload
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(line => JsonSerializer.Deserialize<T>(line))
            .Where(item => item is not null)
            .Cast<T>()
            .ToList();
    }

    private static StringContent CreateSqlContent(string sql) => new(sql, Encoding.UTF8, "text/plain");

    private static string EscapeIdentifier(string identifier) =>
        $"`{identifier.Replace("`", "``", StringComparison.Ordinal)}`";

    private static string EscapeValue(string value) => value
        .Replace("\\", "\\\\", StringComparison.Ordinal)
        .Replace("'", "\\'", StringComparison.Ordinal);

    private sealed class DateRow
    {
        public string Date { get; init; } = string.Empty;
    }

    private sealed class DailyTrendRow
    {
        public string Date { get; init; } = string.Empty;
        public int BranchCardViews { get; init; }
        public int BranchDetailsOpens { get; init; }
        public int TrainingSessionBookings { get; init; }
        public int MembershipPurchases { get; init; }
    }

    private sealed class HourlyTrendRow
    {
        public int Hour { get; init; }
        public int BranchCardViews { get; init; }
        public int BranchDetailsOpens { get; init; }
        public int TrainingSessionBookings { get; init; }
        public int MembershipPurchases { get; init; }
    }

    private sealed class TenMinuteTrendRow
    {
        public int Hour { get; init; }
        public int MinuteBucket { get; init; }
        public int BranchCardViews { get; init; }
        public int BranchDetailsOpens { get; init; }
        public int TrainingSessionBookings { get; init; }
        public int MembershipPurchases { get; init; }
    }
}
