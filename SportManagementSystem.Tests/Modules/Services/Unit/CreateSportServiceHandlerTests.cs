using Moq;
using SportManagementSystem.BuildingBlocks.Time;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Services.Application.Commands.CreateSportService;
using SportManagementSystem.Modules.Services.Contracts.Requests;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Enums;
using SportManagementSystem.Modules.Services.Domain.Repositories;

namespace SportManagementSystem.Tests.Modules.Services.Unit;

public class CreateSportServiceHandlerTests
{
    private readonly Mock<ISportServiceRepository> _sportServiceRepository;
    private readonly Mock<IBranchRepository> _branchRepository;
    private readonly Mock<IAppClock> _clock;
    private readonly CreateSportServiceHandler _handler;

    public CreateSportServiceHandlerTests()
    {
        _sportServiceRepository = new Mock<ISportServiceRepository>();
        _branchRepository = new Mock<IBranchRepository>();
        _clock = new Mock<IAppClock>();
        _clock.SetupGet(x => x.UtcNow).Returns(new DateTimeOffset(2026, 3, 27, 12, 0, 0, TimeSpan.Zero));
        _handler = new CreateSportServiceHandler(_clock.Object, _sportServiceRepository.Object, _branchRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenRequestValid_CreatesService()
    {
        DbSportService? created = null;
        _branchRepository
            .Setup(x => x.GetByIdAsync(2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DbBranch { Id = 2, AdminId = 10, Name = "Branch", Address = "Addr", Latitude = 1, Longitude = 1 });
        _sportServiceRepository
            .Setup(x => x.CreateAsync(It.IsAny<DbSportService>(), It.IsAny<CancellationToken>()))
            .Callback<DbSportService, CancellationToken>((entity, _) => created = entity)
            .ReturnsAsync((DbSportService entity, CancellationToken _) =>
            {
                entity.Id = 44;
                return entity;
            });

        var result = await _handler.Handle(new CreateSportServiceMessage(10, new CreateSportServiceRequest
        {
            BranchId = 2,
            Name = "Yoga",
            Description = "Morning yoga",
            Category = EServiceCategory.Fitness
        }), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(44, result.Data);
        Assert.NotNull(created);
        Assert.Equal("Yoga", created!.Name);
        Assert.True(created.IsActive);
    }
}
