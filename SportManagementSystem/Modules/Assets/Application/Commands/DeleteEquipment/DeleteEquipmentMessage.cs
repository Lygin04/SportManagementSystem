using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;

namespace SportManagementSystem.Modules.Assets.Application.Commands.DeleteEquipment;

public record DeleteEquipmentMessage(long Id) : IMessage<MbResult<Unit>>;
