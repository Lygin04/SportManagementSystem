using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Services.Domain.Entities;

namespace SportManagementSystem.Modules.Services.Application.Queries.GetServicePrice;

public record GetServicePriceMessage(long Id) : IMessage<MbResult<DbServicePrice>>;