namespace SportManagementSystem.Modules.Images.Infrastructure;

public sealed class MinioOptions
{
    public string Endpoint { get; set; } = "minio:9000";
    public string AccessKey { get; set; } = "minioadmin";
    public string SecretKey { get; set; } = "minioadmin";
    public string Bucket { get; set; } = "images";
    public bool UseSsl { get; set; }
}
