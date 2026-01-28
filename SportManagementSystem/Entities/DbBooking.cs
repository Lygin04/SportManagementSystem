using SportManagementSystem.Entities.Enums;
using SportManagementSystem.Modules.Users.Domain.Entities;

namespace SportManagementSystem.Entities;

public class DbBooking
{
    public long Id { get; set; }

    public long? ClientId { get; set; }
    public DbClient? Client { get; set; }

    public long SessionId { get; set; }
    public DbTrainingSession Session { get; set; }

    public DateTime Booked { get; set; }
    public EBookingStatus Status { get; set; }
}