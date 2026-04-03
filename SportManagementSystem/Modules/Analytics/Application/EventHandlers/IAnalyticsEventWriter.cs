namespace SportManagementSystem.Modules.Analytics.Application.EventHandlers;

public interface IAnalyticsEventWriter
{
    Task WriteAsync(AnalyticsEventRecord analyticsEvent, CancellationToken cancellationToken);
}
