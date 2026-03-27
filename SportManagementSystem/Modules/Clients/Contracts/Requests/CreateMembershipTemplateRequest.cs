namespace SportManagementSystem.Modules.Clients.Contracts.Requests;

public class CreateMembershipTemplateRequest
{
    public long BranchId { get; set; }
    public long SportServiceId { get; set; }
    public long ServicePriceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DurationDays { get; set; }
    public int? VisitLimit { get; set; }
}
