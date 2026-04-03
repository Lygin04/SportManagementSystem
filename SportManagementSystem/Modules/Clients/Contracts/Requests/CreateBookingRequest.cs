namespace SportManagementSystem.Modules.Clients.Contracts.Requests;

public class CreateBookingRequest
{
    public long SessionId { get; set; }

    public string? TimeZoneId { get; set; }

    /// <summary>
    /// Дата и время записи с часовым поясом или смещением.
    /// </summary>
    public DateTimeOffset? Booked { get; set; }
}
