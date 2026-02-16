namespace SportManagementSystem.Modules.Images.Contracts.Requests;

public class UploadImageRequest
{
    public IFormFile File { get; set; } = null!;
}
