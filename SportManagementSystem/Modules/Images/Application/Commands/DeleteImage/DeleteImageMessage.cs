using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;

namespace SportManagementSystem.Modules.Images.Application.Commands.DeleteImage;

public record DeleteImageMessage(Guid Id) : IMessage<MbResult<Unit>>;
