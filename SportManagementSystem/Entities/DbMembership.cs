using SportManagementSystem.Entities.Enums;
using SportManagementSystem.Modules.Users.Domain.Entities;

namespace SportManagementSystem.Entities;

/// <summary>
/// Абонемент клиента на услугу.
/// </summary>
public class DbMembership
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public DbClient Client { get; set; }
    
    public long SportServiceId { get; set; }
    public DbSportService SportService { get; set; }
    
    /// <summary>
    /// Дата начала действия.
    /// </summary>
    public DateOnly StartDate { get; set; }
    
    /// <summary>
    /// Дата окончания действия.
    /// </summary>
    public DateOnly EndDate { get; set; }
    
    /// <summary>
    /// Общее количество посещений.
    /// </summary>
    public int TotalVisits { get; set; }
    
    /// <summary>
    /// Оставшееся количество посещений.
    /// </summary>
    public int RemainingVisits { get; set; }
    
    /// <summary>
    /// Статус абонемента.
    /// </summary>
    public EMembershipStatus Status { get; set; }
}