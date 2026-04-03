using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;

namespace SportManagementSystem.Modules.Assets.Application.Commands.DeleteRoom;

public record DeleteRoomMessage(long Id) : IMessage<MbResult<Unit>>;
