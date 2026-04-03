namespace SportManagementSystem.Modules.Clients.Contracts.Response;

public class ClientMembershipResponse
{
    public long Id { get; set; }
    public long SportServiceId { get; set; }
    public string SportServiceName { get; set; } = string.Empty;
    public long BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public long? MembershipTemplateId { get; set; }
    public string? MembershipTemplateName { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int TotalVisits { get; set; }
    public int? RemainingVisits { get; set; }
    public string Status { get; set; } = string.Empty;
}
