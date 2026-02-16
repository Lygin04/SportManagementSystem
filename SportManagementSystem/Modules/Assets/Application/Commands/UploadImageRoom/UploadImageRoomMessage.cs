using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Images.Contracts.Requests;

namespace SportManagementSystem.Modules.Assets.Application.Commands.UploadImageRoom;

public record UploadImageRoomMessage(long RoomId, UploadImageRequest Request) : IMessage<MbResult<Unit>>;
