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
    
    public int BranchId { get; set; }
    public DbBranch Branch { get; set; }
    
    public Guid? AvatarId { get; set; }
    public DbImage? Avatar { get; set; }
}