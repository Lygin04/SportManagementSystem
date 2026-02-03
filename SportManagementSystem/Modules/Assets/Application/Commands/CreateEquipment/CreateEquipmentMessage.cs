using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Contracts.Request;

namespace SportManagementSystem.Modules.Assets.Application.Commands.CreateEquipment;

public record CreateEquipmentMessage(CreateEquipmentRequest Request) : IMessage<MbResult<long>>;