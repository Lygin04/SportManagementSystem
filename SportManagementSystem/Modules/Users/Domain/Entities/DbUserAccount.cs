using SportManagementSystem.Entities.Enums;

namespace SportManagementSystem.Modules.Users.Domain.Entities;

/// <summary>
/// Учетная запись пользователя.
/// </summary>
public class DbUserAccount
{
    public long Id { get; set; }
    
    /// <summary>
    /// Почта для входа.
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Хэш пароль.
    /// </summary>
    public string PasswordHash { get; set; }
    
    /// <summary>
    /// Роль пользователя.
    /// </summary>
    public EUserRole Role { get; set; }
    
    /// <summary>
    /// Статус учетной записи.
    /// </summary>
    public EAccountStatus Status { get; set; }

    public long? StaffId { get; set; }
    public DbStaff? Staff { get; set; }
    
    public long? ClientId { get; set; }
    public DbClient? Client { get; set; }
    
    /// <summary>
    /// Дата и время создания.
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// Дата и время обновления.
    /// </summary>
    public DateTime? Modified { get; set; }
    
    /// <summary>
    /// Дата и время последнего входа.
    /// </summary>
    public DateTime? LastLogin { get; set; }
}