using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Images.Contracts.Requests;

namespace SportManagementSystem.Modules.Assets.Application.Commands.UploadImageBranch;

public record UploadImageBranchMessage(long BranchId, UploadImageRequest Request) : IMessage<MbResult<Unit>>;
