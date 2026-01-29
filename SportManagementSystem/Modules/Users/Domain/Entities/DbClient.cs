using SportManagementSystem.Entities;

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
    public DateTime RegisterDate { get; set; }
    
    /// <summary>
    /// Дата и время последнего изменения данных.
    /// </summary>
    public DateTime? Modified { get; set; }
    

    public ICollection<DbBooking> Bookings { get; set; }
    public ICollection<DbMembership> Memberships { get; set; }
}