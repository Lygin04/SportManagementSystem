using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;

namespace SportManagementSystem.Modules.Assets.Application.Commands.DeleteImageEquipment;

public record DeleteImageEquipmentMessage(long EquipmentId, Guid ImageId) : IMessage<MbResult<Unit>>;
