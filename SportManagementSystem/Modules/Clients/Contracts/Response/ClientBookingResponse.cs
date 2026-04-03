namespace SportManagementSystem.Modules.Clients.Contracts.Response;

public class ClientBookingResponse
{
    public long Id { get; set; }
    public long SessionId { get; set; }
    public long SportServiceId { get; set; }
    public string SportServiceName { get; set; } = string.Empty;
    public long BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string TimeZoneId { get; set; } = string.Empty;
    public DateTimeOffset Booked { get; set; }
    public DateTimeOffset SessionStartedDate { get; set; }
    public DateTimeOffset SessionEndedDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
