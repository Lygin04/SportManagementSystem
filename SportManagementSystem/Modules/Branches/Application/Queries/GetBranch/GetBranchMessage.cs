using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Branches.Domain.Entities;

namespace SportManagementSystem.Modules.Branches.Application.Queries.GetBranch;

public record GetBranchMessage(long Id) : IMessage<MbResult<DbBranch>>;