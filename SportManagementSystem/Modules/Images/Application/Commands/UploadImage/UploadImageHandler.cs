using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Data;
using SportManagementSystem.Modules.Images.Domain.Entities;
using SportManagementSystem.Modules.Images.Domain.Repositories;
using SportManagementSystem.Modules.Images.Infrastructure;

namespace SportManagementSystem.Modules.Images.Application.Commands.UploadImage;

public class UploadImageHandler(
    IMinioClient minioClient,
    IImageRepository imageRepository,
    ApplicationDbContext dbContext,
    MinioOptions options) : IMessageHandler<UploadImageMessage, MbResult<Guid>>
{
    private const long MaxImageSizeBytes = 15 * 1024 * 1024;
    private const int JpegQuality = 80;
    
    public async Task<MbResult<Guid>> Handle(UploadImageMessage request, CancellationToken cancellationToken)
    {
        await using var stream = request.Request.File.OpenReadStream();
        if (stream.Length <= 0)
        {
            return MbResult<Guid>.Failure(new MbError(
                title: "Invalid File",
                status: StatusCodes.Status422UnprocessableEntity,
                detail: "Размер файла должен быть больше 0."));
        }
        
        if (stream.Length > MaxImageSizeBytes)
        {
            return MbResult<Guid>.Failure(new MbError(
                title: "File Too Large",
                status: StatusCodes.Status413PayloadTooLarge,
                detail: "Размер файла превышает 15 MB."));
        }
        
        var extension = Path.GetExtension(request.Request.File.FileName);
        var id = Guid.NewGuid();
        var objectName = $"{id:N}{extension}";

        try
        {
            var prepared = await PrepareContentAsync(
                stream,
                stream.Length,
                request.Request.File.ContentType,
                cancellationToken);
            await using var contentStream = prepared.stream;
            var contentLength = prepared.length;
            var contentType = prepared.contentType;

            var putArgs = new PutObjectArgs()
                .WithBucket(options.Bucket)
                .WithObject(objectName)
                .WithStreamData(contentStream)
                .WithObjectSize(contentLength)
                .WithContentType(contentType);

            await minioClient.PutObjectAsync(putArgs, cancellationToken);

            var image = new DbImage
            {
                Id = id,
                ObjectName = objectName,
                Bucket = options.Bucket,
                FileName = request.Request.File.FileName,
                ContentType = contentType,
                Length = contentLength,
                CreatedAtUtc = DateTime.UtcNow
            };

            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await imageRepository.CreateAsync(image, cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return MbResult<Guid>.Success(id);
        }
        catch (MinioException ex)
        {
            await dbContext.Database.RollbackTransactionAsync(cancellationToken);
            return MbResult<Guid>.Failure(new MbError(
                title: "Upload Failed",
                status: StatusCodes.Status500InternalServerError,
                detail: ex.Message));
        }
        catch (Exception ex)
        {
            try
            {
                await dbContext.Database.RollbackTransactionAsync(cancellationToken);
                var removeArgs = new RemoveObjectArgs()
                    .WithBucket(options.Bucket)
                    .WithObject(objectName);
                await minioClient.RemoveObjectAsync(removeArgs, cancellationToken);
            }
            catch
            {
                // Swallow cleanup errors to avoid masking the original exception.
            }

            return MbResult<Guid>.Failure(new MbError(
                title: "Upload Failed",
                status: StatusCodes.Status500InternalServerError,
                detail: ex.Message));
        }
    }

    private static async Task<(Stream stream, long length, string contentType)> PrepareContentAsync(
        Stream content,
        long length,
        string contentType,
        CancellationToken ct)
    {
        var buffer = new MemoryStream(length <= int.MaxValue ? (int)length : 0);
        await content.CopyToAsync(buffer, ct);
        buffer.Position = 0;

        if (!IsCompressibleContentType(contentType))
        {
            return (buffer, buffer.Length, contentType);
        }

        try
        {
            using var image = await Image.LoadAsync(buffer, ct);
            var compressed = new MemoryStream();

            if (IsJpeg(contentType))
            {
                await image.SaveAsJpegAsync(compressed, new JpegEncoder { Quality = JpegQuality }, ct);
                contentType = "image/jpeg";
            }
            else if (IsPng(contentType))
            {
                await image.SaveAsPngAsync(compressed, new PngEncoder { CompressionLevel = PngCompressionLevel.Level6 }, ct);
            }
            else
            {
                buffer.Position = 0;
                return (buffer, buffer.Length, contentType);
            }

            if (compressed.Length >= buffer.Length)
            {
                await compressed.DisposeAsync();
                buffer.Position = 0;
                return (buffer, buffer.Length, contentType);
            }

            compressed.Position = 0;
            await buffer.DisposeAsync();
            return (compressed, compressed.Length, contentType);
        }
        catch (UnknownImageFormatException)
        {
            buffer.Position = 0;
            return (buffer, buffer.Length, contentType);
        }
        catch (ImageFormatException)
        {
            buffer.Position = 0;
            return (buffer, buffer.Length, contentType);
        }
    }

    private static bool IsCompressibleContentType(string contentType)
        => IsJpeg(contentType) || IsPng(contentType);

    private static bool IsJpeg(string contentType)
        => contentType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase)
           || contentType.Equals("image/jpg", StringComparison.OrdinalIgnoreCase);

    private static bool IsPng(string contentType)
        => contentType.Equals("image/png", StringComparison.OrdinalIgnoreCase);
}
