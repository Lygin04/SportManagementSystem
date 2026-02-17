using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Images.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Enums;
using SportManagementSystem.Modules.Users.Infrastructure.Repositories;
using SportManagementSystem.Tests.Infrastructure;

namespace SportManagementSystem.Tests.Modules.Users.Integration;

[Collection(PostgreSqlCollection.Name)]
public class UsersRepositoriesIntegrationTests(PostgreSqlTestContainerFixture fixture)
{
    [Fact]
    public async Task UserAccountRepository_CreateAndGetByEmail_WorksWithPostgreSql()
    {
        await using var db = fixture.CreateDbContext();
        var repository = new UserAccountRepository(db);
        var email = $"it-user-{Guid.NewGuid():N}@test.local";

        var created = await repository.CreateAsync(new DbUserAccount
        {
            Email = email,
            PasswordHash = "hash",
            Role = EUserRole.Manager,
            Status = EAccountStatus.Active,
            Created = DateTime.UtcNow
        }, CancellationToken.None);

        var fetched = await repository.GetByEmailAsync(email, CancellationToken.None);

        Assert.True(created.Id > 0);
        Assert.NotNull(fetched);
        Assert.Equal(created.Id, fetched!.Id);
        Assert.Equal(email, fetched.Email);
    }

    [Fact]
    public async Task UserAccountRepository_UpdateLastLoginDate_PersistsValue()
    {
        await using var db = fixture.CreateDbContext();
        var repository = new UserAccountRepository(db);
        var email = $"it-login-{Guid.NewGuid():N}@test.local";

        var created = await repository.CreateAsync(new DbUserAccount
        {
            Email = email,
            PasswordHash = "hash",
            Role = EUserRole.Admin,
            Status = EAccountStatus.Active,
            Created = DateTime.UtcNow
        }, CancellationToken.None);

        await repository.UpdateLastLoginDateAsync(created.Id, CancellationToken.None);
        var updated = await repository.GetByIdAsync(created.Id, CancellationToken.None);

        Assert.NotNull(updated);
        Assert.NotNull(updated!.LastLogin);
        Assert.True(updated.LastLogin > created.Created);
    }

    [Fact]
    public async Task ClientRepository_SetImageId_UpdatesAvatar()
    {
        await using var db = fixture.CreateDbContext();
        var repository = new ClientRepository(db);

        var client = await repository.CreateAsync(new DbClient
        {
            FirstName = "Eva",
            LastName = "Stone",
            BirthDate = new DateOnly(1994, 4, 4),
            Phone = "+12345678903",
            RegisterDate = DateTime.UtcNow
        }, CancellationToken.None);

        var imageId = Guid.NewGuid();
        db.Images.Add(new DbImage
        {
            Id = imageId,
            ObjectName = "client-avatar.jpg",
            Bucket = "images",
            FileName = "client-avatar.jpg",
            ContentType = "image/jpeg",
            Length = 10,
            CreatedAtUtc = DateTime.UtcNow
        });
        await db.SaveChangesAsync(CancellationToken.None);

        var updated = await repository.SetImageIdAsync(client.Id, imageId, CancellationToken.None);
        var fetched = await repository.GetByIdAsync(client.Id, CancellationToken.None);

        Assert.True(updated);
        Assert.NotNull(fetched);
        Assert.Equal(imageId, fetched!.AvatarId);
    }

    [Fact]
    public async Task StaffRepository_SetImageId_UpdatesAvatar()
    {
        await using var db = fixture.CreateDbContext();
        var repository = new StaffRepository(db);

        var branch = db.Branches.Add(new DbBranch
        {
            Name = "Branch A",
            Address = "Address 1"
        }).Entity;

        await db.SaveChangesAsync(CancellationToken.None);

        var staff = await repository.CreateAsync(new DbStaff
        {
            FirstName = "Mike",
            LastName = "Trainer",
            BirthDate = new DateOnly(1988, 8, 8),
            Phone = "+12345678904",
            BranchId = branch.Id
        }, CancellationToken.None);

        var imageId = Guid.NewGuid();
        db.Images.Add(new DbImage
        {
            Id = imageId,
            ObjectName = "staff-avatar.jpg",
            Bucket = "images",
            FileName = "staff-avatar.jpg",
            ContentType = "image/jpeg",
            Length = 10,
            CreatedAtUtc = DateTime.UtcNow
        });
        await db.SaveChangesAsync(CancellationToken.None);

        var updated = await repository.SetImageIdAsync(staff.Id, imageId, CancellationToken.None);
        var fetched = await repository.GetByIdAsync(staff.Id, CancellationToken.None);

        Assert.True(updated);
        Assert.NotNull(fetched);
        Assert.Equal(imageId, fetched!.AvatarId);
    }
}
