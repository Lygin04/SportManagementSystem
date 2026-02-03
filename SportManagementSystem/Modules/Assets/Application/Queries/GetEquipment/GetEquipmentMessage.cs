using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Entities;

namespace SportManagementSystem.Modules.Assets.Application.Queries.GetEquipment;

public record GetEquipmentMessage(long Id) : IMessage<MbResult<DbEquipment>>;