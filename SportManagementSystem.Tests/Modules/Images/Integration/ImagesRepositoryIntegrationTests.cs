using SportManagementSystem.Modules.Images.Domain.Entities;
using SportManagementSystem.Modules.Images.Infrastructure.Repositories;
using SportManagementSystem.Tests.Infrastructure;

namespace SportManagementSystem.Tests.Modules.Images.Integration;

[Collection(PostgreSqlCollection.Name)]
public class ImagesRepositoryIntegrationTests(PostgreSqlTestContainerFixture fixture)
{
    [Fact]
    public async Task ImageRepository_CreateAndGetById_Works()
    {
        await using var db = fixture.CreateDbContext();
        var repository = new ImageRepository(db);

        var id = Guid.NewGuid();
        var created = await repository.CreateAsync(new DbImage
        {
            Id = id,
            ObjectName = "file.jpg",
            Bucket = "images",
            FileName = "file.jpg",
            ContentType = "image/jpeg",
            Length = 123
        }, CancellationToken.None);

        var fetched = await repository.GetByIdAsync(id, CancellationToken.None);

        Assert.Equal(id, created.Id);
        Assert.NotNull(fetched);
        Assert.Equal("file.jpg", fetched!.FileName);
    }

    [Fact]
    public async Task ImageRepository_Delete_RemovesEntity()
    {
        await using var db = fixture.CreateDbContext();
        var repository = new ImageRepository(db);

        var id = Guid.NewGuid();
        await repository.CreateAsync(new DbImage
        {
            Id = id,
            ObjectName = "file2.jpg",
            Bucket = "images",
            FileName = "file2.jpg",
            ContentType = "image/jpeg",
            Length = 10
        }, CancellationToken.None);

        await repository.DeleteAsync(id, CancellationToken.None);
        var exists = await repository.ExistsAsync(id, CancellationToken.None);

        Assert.False(exists);
    }
}
