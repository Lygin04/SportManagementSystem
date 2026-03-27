namespace SportManagementSystem.Modules.Clients.Contracts.Response;

public class MembershipTemplateResponse
{
    public long Id { get; set; }
    public long BranchId { get; set; }
    public long SportServiceId { get; set; }
    public long ServicePriceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal PriceAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public int? VisitLimit { get; set; }
    public bool IsActive { get; set; }
    public string SportServiceName { get; set; } = string.Empty;
}
