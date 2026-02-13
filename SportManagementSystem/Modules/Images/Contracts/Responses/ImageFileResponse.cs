namespace SportManagementSystem.Modules.Images.Contracts.Responses;

public class ImageFileResponse(Stream content, string contentType, string fileName, long length)
{
    public Stream Content { get; } = content;
    public string ContentType { get; } = contentType;
    public string FileName { get; } = fileName;
    public long Length { get; } = length;
}
