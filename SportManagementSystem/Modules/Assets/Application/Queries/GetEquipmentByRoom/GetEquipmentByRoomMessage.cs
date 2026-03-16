using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Entities;

namespace SportManagementSystem.Modules.Assets.Application.Queries.GetEquipmentByRoom;

public record GetEquipmentByRoomMessage(long RoomId) : IMessage<MbResult<List<DbEquipment>>>;

