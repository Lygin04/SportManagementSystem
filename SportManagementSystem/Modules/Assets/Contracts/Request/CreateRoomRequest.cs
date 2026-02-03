namespace SportManagementSystem.Modules.Assets.Contracts.Request;

public class CreateRoomRequest
{
    /// <summary>
    /// 
    /// </summary>
    public long BranchId { get; set; }
    
    /// <summary>
    /// Название помещения.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Вместимость помещения.
    /// </summary>
    public int? Capacity { get; set; }
}