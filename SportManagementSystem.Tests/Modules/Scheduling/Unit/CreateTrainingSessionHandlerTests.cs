using Microsoft.AspNetCore.Http;
using Moq;
using SportManagementSystem.BuildingBlocks.Time;
using SportManagementSystem.Modules.Scheduling.Application.Commands.CreateTrainingSession;
using SportManagementSystem.Modules.Scheduling.Contracts.Requests;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Repositories;
using SportManagementSystem.Modules.Services.Domain.Repositories;
using SportManagementSystem.Modules.Users.Domain.Entities;
using SportManagementSystem.Modules.Users.Domain.Enums;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Tests.Modules.Scheduling.Unit;

public class CreateTrainingSessionHandlerTests
{
    private readonly Mock<ITrainingSessionRepository> _trainingSessionRepository;
    private readonly Mock<ISportServiceRepository> _sportServiceRepository;
    private readonly Mock<IServiceScheduleRepository> _serviceScheduleRepository;
    private readonly Mock<IStaffRepository> _staffRepository;
    private readonly Mock<IUserAccountRepository> _userAccountRepository;
    private readonly Mock<IAppClock> _clock;
    private readonly CreateTrainingSessionHandler _handler;

    public CreateTrainingSessionHandlerTests()
    {
        _trainingSessionRepository = new Mock<ITrainingSessionRepository>();
        _sportServiceRepository = new Mock<ISportServiceRepository>();
        _serviceScheduleRepository = new Mock<IServiceScheduleRepository>();
        _staffRepository = new Mock<IStaffRepository>();
        _userAccountRepository = new Mock<IUserAccountRepository>();
        _clock = new Mock<IAppClock>();
        _clock.SetupGet(x => x.DefaultTimeZone).Returns(TimeZoneInfo.Utc);
        _handler = new CreateTrainingSessionHandler(
            _clock.Object,
            _trainingSessionRepository.Object,
            _sportServiceRepository.Object,
            _serviceScheduleRepository.Object,
            _staffRepository.Object,
            _userAccountRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenSportServiceMissing_ReturnsConflict()
    {
        _sportServiceRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _handler.Handle(CreateMessage(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status409Conflict, result.Error!.Status);
        _trainingSessionRepository.Verify(x => x.CreateAsync(It.IsAny<DbTrainingSession>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenTrainerMissing_ReturnsNotFound()
    {
        _sportServiceRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _serviceScheduleRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _staffRepository.Setup(x => x.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync((DbStaff?)null);

        var result = await _handler.Handle(CreateMessage(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status404NotFound, result.Error!.Status);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotTrainer_ReturnsConflict()
    {
        _sportServiceRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _serviceScheduleRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _staffRepository.Setup(x => x.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(new DbStaff { Id = 1, FirstName = "A", LastName = "B", Phone = "+12345678901", BirthDate = new DateOnly(1990, 1, 1), BranchId = 1 });
        _userAccountRepository.Setup(x => x.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(new DbUserAccount
        {
            Email = "manager@test.local",
            PasswordHash = "hash",
            Role = EUserRole.Manager,
            Status = EAccountStatus.Active,
            Created = DateTimeOffset.UtcNow
        });

        var result = await _handler.Handle(CreateMessage(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status409Conflict, result.Error!.Status);
    }

    [Fact]
    public async Task Handle_WhenRequestValid_CreatesTrainingSession()
    {
        DbTrainingSession? created = null;
        _sportServiceRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _serviceScheduleRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _staffRepository.Setup(x => x.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(new DbStaff { Id = 2, FirstName = "T", LastName = "R", Phone = "+12345678901", BirthDate = new DateOnly(1990, 1, 1), BranchId = 1 });
        _userAccountRepository.Setup(x => x.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(new DbUserAccount
        {
            Email = "trainer@test.local",
            PasswordHash = "hash",
            Role = EUserRole.Trainer,
            Status = EAccountStatus.Active,
            Created = DateTimeOffset.UtcNow
        });
        _trainingSessionRepository
            .Setup(x => x.CreateAsync(It.IsAny<DbTrainingSession>(), It.IsAny<CancellationToken>()))
            .Callback<DbTrainingSession, CancellationToken>((entity, _) => created = entity)
            .ReturnsAsync((DbTrainingSession entity, CancellationToken _) =>
            {
                entity.Id = 77;
                return entity;
            });

        var result = await _handler.Handle(CreateMessage(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(77, result.Data);
        Assert.NotNull(created);
        Assert.Equal(5, created!.TrainerId);
        Assert.Equal("UTC", created.TimeZoneId);
    }

    private static CreateTrainingSessionMessage CreateMessage() =>
        new(new CreateTrainingSessionRequest
        {
            SportServiceId = 3,
            ScheduleId = 1,
            TrainerId = 5,
            StartedDate = DateTimeOffset.UtcNow.AddHours(2),
            EndedDate = DateTimeOffset.UtcNow.AddHours(3)
        });
}
