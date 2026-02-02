using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Branches.Domain.Entities;

namespace SportManagementSystem.Modules.Branches.Application.Queries.GetEquipment;

public record GetEquipmentMessage(long Id) : IMessage<MbResult<DbEquipment>>;