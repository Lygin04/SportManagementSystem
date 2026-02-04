namespace SportManagementSystem.Modules.Clients.Contracts.Requests;

public class CreateBookingRequest
{
    public long SessionId { get; set; }

    /// <summary>
    /// Дата и время записи.
    /// </summary>
    public DateTime Booked { get; set; }
}