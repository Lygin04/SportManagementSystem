using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Contracts.Request;

namespace SportManagementSystem.Modules.Assets.Application.Commands.CreateRoom;

public record CreateRoomMessage(CreateRoomRequest Request) : IMessage<MbResult<long>>;