using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Entities;

namespace SportManagementSystem.Modules.Assets.Application.Queries.GetBranches;

public record GetBranchesMessage : IMessage<MbResult<List<DbBranch>>>;