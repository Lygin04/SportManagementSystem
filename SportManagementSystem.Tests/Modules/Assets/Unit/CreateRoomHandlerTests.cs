using Microsoft.AspNetCore.Http;
using Moq;
using SportManagementSystem.Modules.Assets.Application.Commands.CreateRoom;
using SportManagementSystem.Modules.Assets.Contracts.Request;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Enums;
using SportManagementSystem.Modules.Assets.Domain.Repositories;

namespace SportManagementSystem.Tests.Modules.Assets.Unit;

public class CreateRoomHandlerTests
{
    private readonly Mock<IRoomRepository> _roomRepository;
    private readonly Mock<IBranchRepository> _branchRepository;
    private readonly CreateRoomHandler _handler;

    public CreateRoomHandlerTests()
    {
        _roomRepository = new Mock<IRoomRepository>();
        _branchRepository = new Mock<IBranchRepository>();
        _handler = new CreateRoomHandler(_roomRepository.Object, _branchRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenBranchMissing_ReturnsConflict()
    {
        _branchRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _handler.Handle(new CreateRoomMessage(new CreateRoomRequest
        {
            BranchId = 99,
            Name = "Room A",
            Capacity = 10
        }), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status409Conflict, result.Error!.Status);
        _roomRepository.Verify(x => x.CreateAsync(It.IsAny<DbRoom>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRequestValid_CreatesRoom()
    {
        DbRoom? created = null;
        _branchRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _roomRepository
            .Setup(x => x.CreateAsync(It.IsAny<DbRoom>(), It.IsAny<CancellationToken>()))
            .Callback<DbRoom, CancellationToken>((entity, _) => created = entity)
            .ReturnsAsync((DbRoom entity, CancellationToken _) =>
            {
                entity.Id = 33;
                return entity;
            });

        var result = await _handler.Handle(new CreateRoomMessage(new CreateRoomRequest
        {
            BranchId = 5,
            Name = "Room B",
            Capacity = 20
        }), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(33, result.Data);
        Assert.NotNull(created);
        Assert.Equal(ERoomStatus.Available, created!.Status);
    }
}
