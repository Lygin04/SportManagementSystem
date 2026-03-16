using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Entities;

namespace SportManagementSystem.Modules.Assets.Application.Queries.GetBranchesByAdmin;

public record GetBranchesByAdminMessage(long UserId) : IMessage<MbResult<List<DbBranch>>>;