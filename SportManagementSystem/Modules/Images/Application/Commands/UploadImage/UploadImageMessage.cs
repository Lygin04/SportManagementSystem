using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.Modules.Images.Contracts.Requests;

namespace SportManagementSystem.Modules.Images.Application.Commands.UploadImage;

public record UploadImageMessage(UploadImageRequest Request) : IMessage<MbResult<Guid>>;
