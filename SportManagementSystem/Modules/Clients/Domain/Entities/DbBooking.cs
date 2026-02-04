using SportManagementSystem.Entities;
using SportManagementSystem.Entities.Enums;
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

    /// <summary>
    /// Дата и время записи.
    /// </summary>
    public DateTime Booked { get; set; }
    
    /// <summary>
    /// Статус записи.
    /// </summary>
    public EBookingStatus Status { get; set; }
}