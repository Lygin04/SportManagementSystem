using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Images.Contracts.Requests;

namespace SportManagementSystem.Modules.Assets.Application.Commands.UploadImageEquipment;

public record UploadImageEquipmentMessage(long EquipmentId, UploadImageRequest Request) : IMessage<MbResult<Unit>>;
