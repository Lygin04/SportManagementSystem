using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Branches.Domain.Entities;

namespace SportManagementSystem.Modules.Branches.Application.Queries.GetRoom;

public record GetRoomMessage(long Id) : IMessage<MbResult<DbRoom>>;