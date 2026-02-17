using Microsoft.AspNetCore.Http;
using Minio;
using Minio.DataModel.Args;
using Moq;
using SportManagementSystem.Modules.Images.Application.Commands.DeleteImage;
using SportManagementSystem.Modules.Images.Domain.Entities;
using SportManagementSystem.Modules.Images.Domain.Repositories;
using SportManagementSystem.Modules.Images.Infrastructure;

namespace SportManagementSystem.Tests.Modules.Images.Unit;

public class DeleteImageHandlerTests
{
    private readonly Mock<IMinioClient> _minioClient;
    private readonly Mock<IImageRepository> _imageRepository;
    private readonly MinioOptions _options;
    private readonly DeleteImageHandler _handler;

    public DeleteImageHandlerTests()
    {
        _minioClient = new Mock<IMinioClient>();
        _imageRepository = new Mock<IImageRepository>();
        _options = new MinioOptions { Bucket = "images" };
        _handler = new DeleteImageHandler(_minioClient.Object, _imageRepository.Object, _options);
    }

    [Fact]
    public async Task Handle_WhenImageMissing_ReturnsNotFound()
    {
        _imageRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((DbImage?)null);

        var result = await _handler.Handle(new DeleteImageMessage(Guid.NewGuid()), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status404NotFound, result.Error!.Status);
        _minioClient.Verify(x => x.RemoveObjectAsync(It.IsAny<RemoveObjectArgs>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenImageExists_RemovesFromStorageAndDb()
    {
        var id = Guid.NewGuid();
        _imageRepository.Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DbImage { Id = id, ObjectName = "img.png" });
        _minioClient.Setup(x => x.RemoveObjectAsync(It.IsAny<RemoveObjectArgs>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _imageRepository.Setup(x => x.DeleteAsync(id, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(new DeleteImageMessage(id), CancellationToken.None);

        Assert.True(result.IsSuccess);
        _minioClient.Verify(x => x.RemoveObjectAsync(It.IsAny<RemoveObjectArgs>(), It.IsAny<CancellationToken>()), Times.Once);
        _imageRepository.Verify(x => x.DeleteAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }
}
