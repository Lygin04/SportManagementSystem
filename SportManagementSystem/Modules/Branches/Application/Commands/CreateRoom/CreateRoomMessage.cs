using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Branches.Contracts.Request;

namespace SportManagementSystem.Modules.Branches.Application.Commands.CreateRoom;

public record CreateRoomMessage(CreateRoomRequest Request) : IMessage<MbResult<long>>;