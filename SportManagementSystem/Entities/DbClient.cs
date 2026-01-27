using SportManagementSystem.Entities.Enums;

namespace SportManagementSystem.Entities;

public class DbClient
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime RegisterDate { get; set; }
    public DateTime? Modified { get; set; }
    

    public ICollection<DbBooking> Bookings { get; set; }
    public ICollection<DbMembership> Memberships { get; set; }
}