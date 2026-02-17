using Microsoft.AspNetCore.Http;
using Minio;
using Minio.DataModel.Args;
using Moq;
using SportManagementSystem.Modules.Images.Application.Queries.GetImage;
using SportManagementSystem.Modules.Images.Contracts.Responses;
using SportManagementSystem.Modules.Images.Domain.Entities;
using SportManagementSystem.Modules.Images.Domain.Repositories;
using SportManagementSystem.Modules.Images.Infrastructure;

namespace SportManagementSystem.Tests.Modules.Images.Unit;

public class GetImageHandlerTests
{
    private readonly Mock<IMinioClient> _minioClient;
    private readonly Mock<IImageRepository> _imageRepository;
    private readonly MinioOptions _options;
    private readonly GetImageHandler _handler;

    public GetImageHandlerTests()
    {
        _minioClient = new Mock<IMinioClient>();
        _imageRepository = new Mock<IImageRepository>();
        _options = new MinioOptions { Bucket = "images" };
        _handler = new GetImageHandler(_minioClient.Object, _imageRepository.Object, _options);
    }

    [Fact]
    public async Task Handle_WhenImageMissing_ReturnsNotFound()
    {
        _imageRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((DbImage?)null);

        var result = await _handler.Handle(new GetImageMessage(Guid.NewGuid()), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status404NotFound, result.Error!.Status);
        _minioClient.Verify(x => x.GetObjectAsync(It.IsAny<GetObjectArgs>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenImageExists_ReturnsFileResponse()
    {
        var id = Guid.NewGuid();
        _imageRepository.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DbImage
            {
                Id = id,
                ObjectName = "img.jpg",
                Bucket = "images",
                FileName = "img.jpg",
                ContentType = "image/jpeg",
                Length = 10
            });
        _minioClient.Setup(x => x.GetObjectAsync(It.IsAny<GetObjectArgs>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Minio.DataModel.ObjectStat)null!);

        var result = await _handler.Handle(new GetImageMessage(id), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.IsType<ImageFileResponse>(result.Data);
        Assert.Equal("image/jpeg", result.Data!.ContentType);
        Assert.Equal("img.jpg", result.Data.FileName);
        _minioClient.Verify(x => x.GetObjectAsync(It.IsAny<GetObjectArgs>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
