using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Contracts.Request;

namespace SportManagementSystem.Modules.Assets.Application.Commands.CreateBranch;

public record CreateBranchMessage(long AdminId, CreateBranchRequest Request) : IMessage<MbResult<long>>;