using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Images.Contracts.Responses;
using SportManagementSystem.Modules.Images.Domain.Repositories;
using SportManagementSystem.Modules.Images.Infrastructure;

namespace SportManagementSystem.Modules.Images.Application.Queries.GetImage;

public class GetImageHandler(
    IMinioClient minioClient,
    IImageRepository imageRepository,
    MinioOptions options) : IMessageHandler<GetImageMessage, MbResult<ImageFileResponse>>
{
    public async Task<MbResult<ImageFileResponse>> Handle(GetImageMessage request, CancellationToken cancellationToken)
    {
        var image = await imageRepository.GetByIdAsync(request.Id, cancellationToken);
        if (image is null)
        {
            return MbResult<ImageFileResponse>.Failure(new MbError(
                title: "Image Not Found",
                status: StatusCodes.Status404NotFound,
                detail: "Изображение не найдено."));
        }

        try
        {
            var memoryStream = new MemoryStream();
            var getArgs = new GetObjectArgs()
                .WithBucket(options.Bucket)
                .WithObject(image.ObjectName)
                .WithCallbackStream(stream =>
                {
                    stream.CopyTo(memoryStream);
                });

            await minioClient.GetObjectAsync(getArgs, cancellationToken);
            memoryStream.Position = 0;

            var response = new ImageFileResponse(
                content: memoryStream,
                contentType: image.ContentType,
                fileName: image.FileName,
                length: image.Length);

            return MbResult<ImageFileResponse>.Success(response);
        }
        catch (MinioException)
        {
            return MbResult<ImageFileResponse>.Failure(new MbError(
                title: "Image Not Found",
                status: StatusCodes.Status404NotFound,
                detail: "Изображение не найдено."));
        }
    }
}
