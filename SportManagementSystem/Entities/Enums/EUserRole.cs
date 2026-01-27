using System.Text.Json.Serialization;

namespace SportManagementSystem.Entities.Enums;

/// <summary>
/// Роль пользователя системы (сотрудника).
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EUserRole
{
    /// <summary>
    /// Администратор.
    /// </summary>
    Admin,
    
    /// <summary>
    /// Менеджер спортивных организаций.
    /// </summary>
    Manager,
    
    /// <summary>
    /// Тренер, специалист по услуге.
    /// </summary>
    Trainer,
    
    /// <summary>
    /// Кассир, сотрудник по работе с оплатми.
    /// </summary>
    Cashier,
    
    /// <summary>
    /// Клиент системы.
    /// </summary>
    Client
}