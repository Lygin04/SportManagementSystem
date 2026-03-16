namespace SportManagementSystem.Modules.Services.Contracts.Response;

public class ServicePriceResponse
{
    public long Id { get; set; }
    public long SportServiceId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateOnly ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
}
