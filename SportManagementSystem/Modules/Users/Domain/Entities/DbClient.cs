using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Images.Domain.Entities;

namespace SportManagementSystem.Modules.Users.Domain.Entities;

/// <summary>
/// Клиент спортивной организации.
/// </summary>
public class DbClient
{
    public long Id { get; set; }
    
    /// <summary>
    /// Имя.
    /// </summary>
    public string FirstName { get; set; }
    
    /// <summary>
    /// Фамилия.
    /// </summary>
    public string LastName { get; set; }
    
    /// <summary>
    /// Отчество.
    /// </summary>
    public string? Patronymic { get; set; }
    
    /// <summary>
    /// Дата рождения.
    /// </summary>
    public DateOnly BirthDate { get; set; }
    
    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string Phone { get; set; }
    
    /// <summary>
    /// Дата и время регистрации в системе.
    /// </summary>
    public DateTimeOffset RegisterDate { get; set; }
    
    /// <summary>
    /// Дата и время последнего изменения данных.
    /// </summary>
    public DateTimeOffset? Modified { get; set; }
    
    public Guid? AvatarId { get; set; }
    public DbImage? Avatar { get; set; }

    public ICollection<DbBooking> Bookings { get; set; }
    public ICollection<DbMembership> Memberships { get; set; }
}
