using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Images.Domain.Entities;

namespace SportManagementSystem.Modules.Users.Domain.Entities;

/// <summary>
/// Сотрудник спортивной организации.
/// </summary>
public class DbStaff
{
    /// <summary>
    /// Уникальный идентификатор сотрудника.
    /// </summary>
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
    /// Номер телефона.
    /// </summary>
    public string Phone { get; set; }
    
    /// <summary>
    /// Дата рождения.
    /// </summary>
    public DateOnly BirthDate { get; set; }
    
    // Branches where this employee is assigned as administrator.
    public ICollection<DbBranch> AdminBranches { get; set; } = new List<DbBranch>();

    // Branches where this employee works as staff.
    public ICollection<DbBranch> StaffBranches { get; set; } = new List<DbBranch>();

    // Branches created by this employee as the main administrator.
    public ICollection<DbBranch> CreatedBranches { get; set; } = new List<DbBranch>();
    
    public Guid? AvatarId { get; set; }
    public DbImage? Avatar { get; set; }
}
