using Microsoft.AspNetCore.Http;
using Moq;
using SportManagementSystem.Modules.Assets.Application.Commands.CreateEquipment;
using SportManagementSystem.Modules.Assets.Contracts.Request;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Enums;
using SportManagementSystem.Modules.Assets.Domain.Repositories;

namespace SportManagementSystem.Tests.Modules.Assets.Unit;

public class CreateEquipmentHandlerTests
{
    private readonly Mock<IEquipmentRepository> _equipmentRepository;
    private readonly Mock<IRoomRepository> _roomRepository;
    private readonly CreateEquipmentHandler _handler;

    public CreateEquipmentHandlerTests()
    {
        _equipmentRepository = new Mock<IEquipmentRepository>();
        _roomRepository = new Mock<IRoomRepository>();
        _handler = new CreateEquipmentHandler(_equipmentRepository.Object, _roomRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenRoomMissing_ReturnsConflict()
    {
        _roomRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _handler.Handle(new CreateEquipmentMessage(new CreateEquipmentRequest
        {
            RoomId = 8,
            Name = "Treadmill",
            Quantity = 2,
            Condition = EEquipmentCondition.New
        }), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status409Conflict, result.Error!.Status);
        _equipmentRepository.Verify(x => x.CreateAsync(It.IsAny<DbEquipment>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRequestValid_CreatesEquipment()
    {
        DbEquipment? created = null;
        _roomRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _equipmentRepository
            .Setup(x => x.CreateAsync(It.IsAny<DbEquipment>(), It.IsAny<CancellationToken>()))
            .Callback<DbEquipment, CancellationToken>((entity, _) => created = entity)
            .ReturnsAsync((DbEquipment entity, CancellationToken _) =>
            {
                entity.Id = 77;
                return entity;
            });

        var result = await _handler.Handle(new CreateEquipmentMessage(new CreateEquipmentRequest
        {
            RoomId = 3,
            Name = "Bike",
            Quantity = 5,
            Condition = EEquipmentCondition.Good
        }), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(77, result.Data);
        Assert.NotNull(created);
        Assert.Equal("Bike", created!.Name);
        Assert.Equal(EEquipmentCondition.Good, created.Condition);
    }
}
