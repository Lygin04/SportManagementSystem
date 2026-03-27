using SportManagementSystem.Modules.Clients.Domain.Enums;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Entities;

namespace SportManagementSystem.Modules.Clients.Domain.Entities;

/// <summary>
/// Абонемент клиента на услугу.
/// </summary>
public class DbMembership
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public DbClient Client { get; set; }

    public long? MembershipTemplateId { get; set; }
    public DbMembershipTemplate? MembershipTemplate { get; set; }
    
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
    public int? RemainingVisits { get; set; }
    
    /// <summary>
    /// Статус абонемента.
    /// </summary>
    public EMembershipStatus Status { get; set; }
}
