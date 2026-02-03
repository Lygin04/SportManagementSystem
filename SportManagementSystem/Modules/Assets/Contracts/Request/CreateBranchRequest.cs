namespace SportManagementSystem.Modules.Assets.Contracts.Request;

public class CreateBranchRequest
{
    /// <summary>
    /// Название Филиала.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Физический адрес.
    /// </summary>
    public string? Address { get; set; }
    
    /// <summary>
    /// Географическая широта.
    /// </summary>
    public double? Latitude { get; set; }
    
    /// <summary>
    /// Географическая долгота.
    /// </summary>
    public double? Longitude { get; set; }
}