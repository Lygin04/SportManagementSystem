namespace SportManagementSystem.Modules.Clients.Contracts.Requests;

public class CreateMembershipRequest
{
    public long SportServiceId { get; set; }
    
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
    public int? RemainingVisits { get; set; }
}