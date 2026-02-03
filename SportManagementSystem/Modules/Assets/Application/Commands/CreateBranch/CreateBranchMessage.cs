using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Contracts.Request;

namespace SportManagementSystem.Modules.Assets.Application.Commands.CreateBranch;

public record CreateBranchMessage(CreateBranchRequest Request) : IMessage<MbResult<long>>;