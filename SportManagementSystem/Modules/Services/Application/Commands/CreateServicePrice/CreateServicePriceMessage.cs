using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Services.Contracts.Requests;

namespace SportManagementSystem.Modules.Services.Application.Commands.CreateServicePrice;

public record CreateServicePriceMessage(CreateServicePriceRequest Request) : IMessage<MbResult<long>>;