using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;

namespace SportManagementSystem.Modules.Assets.Application.Commands.DeleteImageBranch;

public record DeleteImageBranchMessage(long BranchId, Guid ImageId) : IMessage<MbResult<Unit>>;
