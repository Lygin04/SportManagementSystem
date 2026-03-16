using SportManagementSystem.Modules.Services.Contracts.Response;

namespace SportManagementSystem.Modules.Services.Contracts.Response;

public class SportServiceResponse
{
    public long Id { get; set; }
    public long BranchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public List<ServicePriceResponse> Prices { get; set; } = [];
}
