using SportManagementSystem.Entities.Enums;

namespace SportManagementSystem.Entities;

public class DbMembership
{
    public long Id { get; set; }
    public long ClientId { get; set; }
    public DbClient Client { get; set; }
    
    public long SportServiceId { get; set; }
    public DbSportService SportService { get; set; }
    
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    
    public int TotalVisits { get; set; }
    public int RemainingVisits { get; set; }
    public EMembershipStatus Status { get; set; }
}