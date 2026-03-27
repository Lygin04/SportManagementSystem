using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Entities;

namespace SportManagementSystem.Modules.Clients.Domain.Entities;

public class DbMembershipTemplate
{
    public long Id { get; set; }
    public long BranchId { get; set; }
    public DbBranch Branch { get; set; } = null!;

    public long SportServiceId { get; set; }
    public DbSportService SportService { get; set; } = null!;

    public long ServicePriceId { get; set; }
    public DbServicePrice ServicePrice { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationDays { get; set; }
    public int? VisitLimit { get; set; }
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }

    public ICollection<DbMembership> Memberships { get; set; } = new List<DbMembership>();
}
