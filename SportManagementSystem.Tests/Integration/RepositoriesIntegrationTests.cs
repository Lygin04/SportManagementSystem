using SportManagementSystem.Entities.Enums;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Infrastructure.Repositories;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Infrastructure.Repositories;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Modules.Users.Infrastructure.Repositories;

namespace SportManagementSystem.Tests.Integration;

[Collection(PostgreSqlCollection.Name)]
public class RepositoriesIntegrationTests(PostgreSqlTestContainerFixture fixture)
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
    public async Task MembershipRepository_CreateAndGetById_WorksWithRelations()
    {
        await using var db = fixture.CreateDbContext();
        var membershipRepository = new MembershipRepository(db);
        var sportServiceRepository = new SportServiceRepository(db);
        var code = $"svc-{Guid.NewGuid():N}".Substring(0, 16);

        var client = db.Clients.Add(new DbClient
        {
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateOnly(1995, 1, 1),
            Phone = $"+1{Random.Shared.NextInt64(1000000000, 9999999999)}",
            RegisterDate = DateTime.UtcNow
        }).Entity;

        var sportService = await sportServiceRepository.CreateAsync(new DbSportService
        {
            Code = code,
            Name = "Yoga",
            Category = EServiceCategory.Fitness,
            IsActive = true,
            Created = DateTime.UtcNow
        }, CancellationToken.None);

        await db.SaveChangesAsync(CancellationToken.None);

        var created = await membershipRepository.CreateAsync(new DbMembership
        {
            ClientId = client.Id,
            SportServiceId = sportService.Id,
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(31)),
            TotalVisits = 0,
            RemainingVisits = 8,
            Status = EMembershipStatus.Active
        }, CancellationToken.None);

        var fetched = await membershipRepository.GetByIdAsync(created.Id, CancellationToken.None);

        Assert.True(created.Id > 0);
        Assert.NotNull(fetched);
        Assert.Equal(client.Id, fetched!.ClientId);
        Assert.Equal(sportService.Id, fetched.SportServiceId);
        Assert.Equal(EMembershipStatus.Active, fetched.Status);
    }
}
