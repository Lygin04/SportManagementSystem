using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Images.Contracts.Requests;

namespace SportManagementSystem.Modules.Users.Application.Commands.UploadAvatarClient;

public record UploadAvatarClientMessage(long UserId, UploadImageRequest Request) : IMessage<MbResult<Unit>>;