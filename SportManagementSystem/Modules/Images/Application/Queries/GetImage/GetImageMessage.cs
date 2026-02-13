using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Images.Contracts.Responses;

namespace SportManagementSystem.Modules.Images.Application.Queries.GetImage;

public record GetImageMessage(Guid Id) : IMessage<MbResult<ImageFileResponse>>;
