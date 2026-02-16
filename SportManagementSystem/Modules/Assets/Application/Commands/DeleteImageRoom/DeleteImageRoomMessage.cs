using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;

namespace SportManagementSystem.Modules.Assets.Application.Commands.DeleteImageRoom;

public record DeleteImageRoomMessage(long RoomId, Guid ImageId) : IMessage<MbResult<Unit>>;
