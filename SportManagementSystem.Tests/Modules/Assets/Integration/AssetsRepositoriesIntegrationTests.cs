using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Enums;
using SportManagementSystem.Modules.Assets.Infrastructure.Repositories;
using SportManagementSystem.Tests.Infrastructure;

namespace SportManagementSystem.Tests.Modules.Assets.Integration;

[Collection(PostgreSqlCollection.Name)]
public class AssetsRepositoriesIntegrationTests(PostgreSqlTestContainerFixture fixture)
{
    [Fact]
    public async Task BranchRepository_CreateAndGetById_Works()
    {
        await using var db = fixture.CreateDbContext();
        var repository = new BranchRepository(db);

        var created = await repository.CreateAsync(new DbBranch
        {
            Name = "North",
            Address = "Street 9"
        }, CancellationToken.None);

        var fetched = await repository.GetByIdAsync(created.Id, CancellationToken.None);

        Assert.True(created.Id > 0);
        Assert.NotNull(fetched);
        Assert.Equal("North", fetched!.Name);
    }

    [Fact]
    public async Task RoomRepository_GetByBranch_ReturnsRooms()
    {
        await using var db = fixture.CreateDbContext();
        var branchRepository = new BranchRepository(db);
        var roomRepository = new RoomRepository(db);

        var branch = await branchRepository.CreateAsync(new DbBranch
        {
            Name = "East",
            Address = "Street 5"
        }, CancellationToken.None);

        await roomRepository.CreateAsync(new DbRoom
        {
            BranchId = branch.Id,
            Name = "Room X",
            Capacity = 12,
            Status = ERoomStatus.Available
        }, CancellationToken.None);

        var rooms = await roomRepository.GetByBranchAsync(branch.Id, CancellationToken.None);

        Assert.NotNull(rooms);
        Assert.Single(rooms!);
        Assert.Equal(branch.Id, rooms![0].BranchId);
    }

    [Fact]
    public async Task EquipmentRepository_CreateAndGetById_Works()
    {
        await using var db = fixture.CreateDbContext();
        var branchRepository = new BranchRepository(db);
        var roomRepository = new RoomRepository(db);
        var equipmentRepository = new EquipmentRepository(db);

        var branch = await branchRepository.CreateAsync(new DbBranch
        {
            Name = "South",
            Address = "Street 3"
        }, CancellationToken.None);

        var room = await roomRepository.CreateAsync(new DbRoom
        {
            BranchId = branch.Id,
            Name = "Room Z",
            Capacity = 8,
            Status = ERoomStatus.Available
        }, CancellationToken.None);

        var created = await equipmentRepository.CreateAsync(new DbEquipment
        {
            RoomId = room.Id,
            Name = "Rope",
            Quantity = 3,
            Condition = EEquipmentCondition.New
        }, CancellationToken.None);

        var fetched = await equipmentRepository.GetByIdAsync(created.Id, CancellationToken.None);

        Assert.True(created.Id > 0);
        Assert.NotNull(fetched);
        Assert.Equal(room.Id, fetched!.RoomId);
        Assert.Equal("Rope", fetched.Name);
    }
}
