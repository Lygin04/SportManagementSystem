using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Branches.Domain.Entities;

namespace SportManagementSystem.Modules.Branches.Application.Queries.GetRoomByBranch;

public record GetRoomByBranchMessage(long BranchId) : IMessage<MbResult<List<DbRoom>>>;