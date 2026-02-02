using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Branches.Contracts.Request;

namespace SportManagementSystem.Modules.Branches.Application.Commands.CreateEquipment;

public record CreateEquipmentMessage(CreateEquipmentRequest Request) : IMessage<MbResult<long>>;