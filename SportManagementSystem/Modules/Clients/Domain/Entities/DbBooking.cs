using SportManagementSystem.Modules.Clients.Domain.Enums;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Entities;

namespace SportManagementSystem.Modules.Clients.Domain.Entities;

/// <summary>
/// Запись клиента на занятие.
/// </summary>
public class DbBooking
{
    public long Id { get; set; }

    public long? ClientId { get; set; }
    public DbClient? Client { get; set; }

    public long SessionId { get; set; }
    public DbTrainingSession Session { get; set; }

    public string TimeZoneId { get; set; } = "UTC";

    /// <summary>
    /// Дата и время записи.
    /// </summary>
    public DateTimeOffset Booked { get; set; }
    
    /// <summary>
    /// Статус записи.
    /// </summary>
    public EBookingStatus Status { get; set; }
}
