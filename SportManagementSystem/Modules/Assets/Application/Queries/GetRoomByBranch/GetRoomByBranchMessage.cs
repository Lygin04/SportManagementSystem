using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Entities;

namespace SportManagementSystem.Modules.Assets.Application.Queries.GetRoomByBranch;

public record GetRoomByBranchMessage(long BranchId) : IMessage<MbResult<List<DbRoom>>>;