using MediatR;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Images.Domain.Repositories;
using SportManagementSystem.Modules.Images.Infrastructure;

namespace SportManagementSystem.Modules.Images.Application.Commands.DeleteImage;

public class DeleteImageHandler(
    IMinioClient minioClient,
    IImageRepository imageRepository,
    MinioOptions options) : IMessageHandler<DeleteImageMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(DeleteImageMessage request, CancellationToken cancellationToken)
    {
        var image = await imageRepository.GetByIdAsync(request.Id, cancellationToken);
        if (image is null)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Image Not Found",
                status: StatusCodes.Status404NotFound,
                detail: "Изображение не найдено."));
        }

        try
        {
            var args = new RemoveObjectArgs()
                .WithBucket(options.Bucket)
                .WithObject(image.ObjectName);

            await Task.WhenAll(
                minioClient.RemoveObjectAsync(args, cancellationToken),
                imageRepository.DeleteAsync(request.Id, cancellationToken));

            return MbResult<Unit>.Success(Unit.Value);
        }
        catch (MinioException)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Image Not Found",
                status: StatusCodes.Status404NotFound,
                detail: "Изображение не найдено."));
        }
    }
}
