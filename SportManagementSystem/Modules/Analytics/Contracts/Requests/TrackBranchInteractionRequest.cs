namespace SportManagementSystem.Modules.Analytics.Contracts.Requests;

public class TrackBranchInteractionRequest
{
    public long BranchId { get; set; }
    public string ActionType { get; set; } = string.Empty;
}
