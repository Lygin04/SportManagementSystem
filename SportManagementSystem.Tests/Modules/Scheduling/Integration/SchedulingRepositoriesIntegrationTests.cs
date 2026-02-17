using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Enums;
using SportManagementSystem.Modules.Scheduling.Infrastructure.Repositories;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Enums;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Tests.Infrastructure;

namespace SportManagementSystem.Tests.Modules.Scheduling.Integration;

[Collection(PostgreSqlCollection.Name)]
public class SchedulingRepositoriesIntegrationTests(PostgreSqlTestContainerFixture fixture)
{
    [Fact]
    public async Task ServiceScheduleRepository_CreateAndGetById_Works()
    {
        await using var db = fixture.CreateDbContext();
        var repository = new ServiceScheduleRepository(db);

        var branch = db.Branches.Add(new DbBranch
        {
            Name = "Branch",
            Address = "Street"
        }).Entity;

        await db.SaveChangesAsync(CancellationToken.None);

        var room = db.Rooms.Add(new DbRoom
        {
            BranchId = branch.Id,
            Name = "Room 1",
            Capacity = 10
        }).Entity;

        var service = db.SportServices.Add(new DbSportService
        {
            Code = $"svc-{Guid.NewGuid():N}".Substring(0, 12),
            Name = "Stretching",
            Category = EServiceCategory.Fitness,
            IsActive = true,
            Created = DateTime.UtcNow
        }).Entity;

        await db.SaveChangesAsync(CancellationToken.None);

        var created = await repository.CreateAsync(new DbServiceSchedule
        {
            SportServiceId = service.Id,
            StaffId = null,
            RoomId = room.Id,
            DayOfWeek = 3,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(10, 0),
            Note = "Morning"
        }, CancellationToken.None);

        var fetched = await repository.GetByIdAsync(created.Id, CancellationToken.None);

        Assert.True(created.Id > 0);
        Assert.NotNull(fetched);
        Assert.Equal(service.Id, fetched!.SportServiceId);
        Assert.Equal(room.Id, fetched.RoomId);
    }

    [Fact]
    public async Task TrainingSessionRepository_MarkDone_UpdatesStatus()
    {
        await using var db = fixture.CreateDbContext();
        var repository = new TrainingSessionRepository(db);

        var branch = db.Branches.Add(new DbBranch
        {
            Name = "Branch 2",
            Address = "Street 2"
        }).Entity;

        await db.SaveChangesAsync(CancellationToken.None);

        var staff = db.Staffs.Add(new DbStaff
        {
            FirstName = "Sam",
            LastName = "Trainer",
            BirthDate = new DateOnly(1985, 5, 5),
            Phone = "+12345678905",
            BranchId = branch.Id
        }).Entity;

        var service = db.SportServices.Add(new DbSportService
        {
            Code = $"svc-{Guid.NewGuid():N}".Substring(0, 12),
            Name = "Box",
            Category = EServiceCategory.Fitness,
            IsActive = true,
            Created = DateTime.UtcNow
        }).Entity;

        await db.SaveChangesAsync(CancellationToken.None);

        var session = await repository.CreateAsync(new DbTrainingSession
        {
            SportServiceId = service.Id,
            TrainerId = staff.Id,
            StartedDate = DateTime.UtcNow.AddHours(-3),
            EndedDate = DateTime.UtcNow.AddHours(-1),
            Status = ESessionStatus.Planned
        }, CancellationToken.None);

        var updatedCount = await repository.MarkDoneAsync(CancellationToken.None);
        var updated = await repository.GetByIdAsync(session.Id, CancellationToken.None);

        Assert.True(updatedCount > 0);
        Assert.NotNull(updated);
        Assert.Equal(ESessionStatus.Done, updated!.Status);
    }
}
