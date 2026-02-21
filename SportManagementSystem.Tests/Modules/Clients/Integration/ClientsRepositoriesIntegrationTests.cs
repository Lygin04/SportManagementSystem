using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Enums;
using SportManagementSystem.Modules.Clients.Infrastructure.Repositories;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Enums;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Enums;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Tests.Infrastructure;

namespace SportManagementSystem.Tests.Modules.Clients.Integration;

[Collection(PostgreSqlCollection.Name)]
public class ClientsRepositoriesIntegrationTests(PostgreSqlTestContainerFixture fixture)
{
    [Fact]
    public async Task MembershipRepository_CreateAndGetById_Works()
    {
        await using var db = fixture.CreateDbContext();
        var membershipRepository = new MembershipRepository(db);

        var client = db.Clients.Add(new DbClient
        {
            FirstName = "John",
            LastName = "Doe",
            BirthDate = new DateOnly(1995, 1, 1),
            Phone = "+12345678901",
            RegisterDate = DateTime.UtcNow
        }).Entity;

        var service = db.SportServices.Add(new DbSportService
        {
            Code = $"svc-{Guid.NewGuid():N}".Substring(0, 12),
            Name = "Yoga",
            Category = EServiceCategory.Fitness,
            IsActive = true,
            Created = DateTime.UtcNow
        }).Entity;

        await db.SaveChangesAsync(CancellationToken.None);

        var created = await membershipRepository.CreateAsync(new DbMembership
        {
            ClientId = client.Id,
            SportServiceId = service.Id,
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
        Assert.Equal(service.Id, fetched.SportServiceId);
        Assert.Equal(EMembershipStatus.Active, fetched.Status);
    }

    [Fact]
    public async Task BookingRepository_CreateAndGetById_Works()
    {
        await using var db = fixture.CreateDbContext();
        var bookingRepository = new BookingRepository(db);

        var branch = db.Branches.Add(new DbBranch
        {
            Name = "Main",
            Address = "Street 1"
        }).Entity;

        await db.SaveChangesAsync(CancellationToken.None);

        var staff = db.Staffs.Add(new DbStaff
        {
            FirstName = "Alice",
            LastName = "Trainer",
            Phone = "+12345678902",
            BirthDate = new DateOnly(1990, 2, 2),
            BranchId = branch.Id
        }).Entity;

        var service = db.SportServices.Add(new DbSportService
        {
            Code = $"svc-{Guid.NewGuid():N}".Substring(0, 12),
            Name = "Pilates",
            Category = EServiceCategory.Fitness,
            IsActive = true,
            Created = DateTime.UtcNow
        }).Entity;

        await db.SaveChangesAsync(CancellationToken.None);

        var session = db.TrainingSessions.Add(new DbTrainingSession
        {
            SportServiceId = service.Id,
            StartedDate = DateTime.UtcNow.AddHours(2),
            EndedDate = DateTime.UtcNow.AddHours(3),
            TrainerId = staff.Id,
            Status = ESessionStatus.Planned
        }).Entity;

        await db.SaveChangesAsync(CancellationToken.None);

        var created = await bookingRepository.CreateAsync(new DbBooking
        {
            ClientId = null,
            SessionId = session.Id,
            Booked = DateTime.UtcNow.AddHours(1),
            Status = EBookingStatus.Booked
        }, CancellationToken.None);

        var fetched = await bookingRepository.GetByIdAsync(created.Id, CancellationToken.None);

        Assert.True(created.Id > 0);
        Assert.NotNull(fetched);
        Assert.Equal(session.Id, fetched!.SessionId);
        Assert.Equal(EBookingStatus.Booked, fetched.Status);
    }
}
