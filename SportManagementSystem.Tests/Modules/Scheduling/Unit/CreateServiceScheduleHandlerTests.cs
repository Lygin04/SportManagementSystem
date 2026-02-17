using Microsoft.AspNetCore.Http;
using Moq;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Scheduling.Application.Commands.CreateServiceSchedule;
using SportManagementSystem.Modules.Scheduling.Contracts.Requests;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Repositories;
using SportManagementSystem.Modules.Services.Domain.Repositories;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Tests.Modules.Scheduling.Unit;

public class CreateServiceScheduleHandlerTests
{
    private readonly Mock<IServiceScheduleRepository> _serviceScheduleRepository;
    private readonly Mock<ISportServiceRepository> _sportServiceRepository;
    private readonly Mock<IStaffRepository> _staffRepository;
    private readonly Mock<IRoomRepository> _roomRepository;
    private readonly CreateServiceScheduleHandler _handler;

    public CreateServiceScheduleHandlerTests()
    {
        _serviceScheduleRepository = new Mock<IServiceScheduleRepository>();
        _sportServiceRepository = new Mock<ISportServiceRepository>();
        _staffRepository = new Mock<IStaffRepository>();
        _roomRepository = new Mock<IRoomRepository>();
        _handler = new CreateServiceScheduleHandler(
            _serviceScheduleRepository.Object,
            _sportServiceRepository.Object,
            _staffRepository.Object,
            _roomRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenSportServiceMissing_ReturnsConflict()
    {
        _sportServiceRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _handler.Handle(CreateMessage(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status409Conflict, result.Error!.Status);
        _serviceScheduleRepository.Verify(x => x.CreateAsync(It.IsAny<DbServiceSchedule>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenStaffMissing_ReturnsConflict()
    {
        _sportServiceRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _staffRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _roomRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await _handler.Handle(CreateMessage(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status409Conflict, result.Error!.Status);
    }

    [Fact]
    public async Task Handle_WhenRequestValid_CreatesSchedule()
    {
        DbServiceSchedule? created = null;
        _sportServiceRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _staffRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _roomRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _serviceScheduleRepository
            .Setup(x => x.CreateAsync(It.IsAny<DbServiceSchedule>(), It.IsAny<CancellationToken>()))
            .Callback<DbServiceSchedule, CancellationToken>((entity, _) => created = entity)
            .ReturnsAsync((DbServiceSchedule entity, CancellationToken _) =>
            {
                entity.Id = 321;
                return entity;
            });

        var result = await _handler.Handle(CreateMessage(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(321, result.Data);
        Assert.NotNull(created);
        Assert.Equal(2, created!.DayOfWeek);
    }

    private static CreateServiceScheduleMessage CreateMessage() =>
        new(new CreateServiceScheduleRequest
        {
            SportServiceId = 12,
            StaffId = 7,
            RoomId = 4,
            DayOfWeek = 2,
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(9, 0),
            Note = "Morning class"
        });
}
