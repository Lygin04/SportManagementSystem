using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Enums;
using SportManagementSystem.Modules.Services.Infrastructure.Repositories;
using SportManagementSystem.Tests.Infrastructure;

namespace SportManagementSystem.Tests.Modules.Services.Integration;

[Collection(PostgreSqlCollection.Name)]
public class ServicesRepositoriesIntegrationTests(PostgreSqlTestContainerFixture fixture)
{
    [Fact]
    public async Task SportServiceRepository_CreateAndGetById_Works()
    {
        await using var db = fixture.CreateDbContext();
        var repository = new SportServiceRepository(db);

        var created = await repository.CreateAsync(new DbSportService
        {
            Code = $"svc-{Guid.NewGuid():N}".Substring(0, 12),
            Name = "Boxing",
            Category = EServiceCategory.Fitness,
            IsActive = true,
            Created = DateTime.UtcNow
        }, CancellationToken.None);

        var fetched = await repository.GetByIdAsync(created.Id, CancellationToken.None);

        Assert.True(created.Id > 0);
        Assert.NotNull(fetched);
        Assert.Equal("Boxing", fetched!.Name);
    }

    [Fact]
    public async Task ServicePriceRepository_CreateAndGetById_Works()
    {
        await using var db = fixture.CreateDbContext();
        var sportServiceRepository = new SportServiceRepository(db);
        var priceRepository = new ServicePriceRepository(db);

        var service = await sportServiceRepository.CreateAsync(new DbSportService
        {
            Code = $"svc-{Guid.NewGuid():N}".Substring(0, 12),
            Name = "Crossfit",
            Category = EServiceCategory.Fitness,
            IsActive = true,
            Created = DateTime.UtcNow
        }, CancellationToken.None);

        var created = await priceRepository.CreateAsync(new DbServicePrice
        {
            SportServiceId = service.Id,
            Amount = 120,
            Currency = EIsoCurrency.USD,
            ValidFrom = new DateOnly(2026, 1, 1)
        }, CancellationToken.None);

        var fetched = await priceRepository.GetByIdAsync(created.Id, CancellationToken.None);

        Assert.True(created.Id > 0);
        Assert.NotNull(fetched);
        Assert.Equal(service.Id, fetched!.SportServiceId);
        Assert.Equal(EIsoCurrency.USD, fetched.Currency);
    }
}
