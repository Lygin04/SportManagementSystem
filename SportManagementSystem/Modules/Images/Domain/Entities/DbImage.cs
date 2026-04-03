namespace SportManagementSystem.Modules.Images.Domain.Entities;

public class DbImage
{
    public Guid Id { get; set; }
    public string ObjectName { get; set; } = null!;
    public string Bucket { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public long Length { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
}
