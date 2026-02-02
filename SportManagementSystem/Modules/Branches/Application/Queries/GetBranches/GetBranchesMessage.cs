using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Branches.Domain.Entities;

namespace SportManagementSystem.Modules.Branches.Application.Queries.GetBranches;

public record GetBranchesMessage : IMessage<MbResult<List<DbBranch>>>;