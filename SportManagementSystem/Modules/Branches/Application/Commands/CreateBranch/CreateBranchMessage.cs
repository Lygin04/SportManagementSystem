using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Branches.Contracts.Request;

namespace SportManagementSystem.Modules.Branches.Application.Commands.CreateBranch;

public record CreateBranchMessage(CreateBranchRequest Request) : IMessage<MbResult<long>>;