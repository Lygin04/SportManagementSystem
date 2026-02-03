using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Services.Contracts.Requests;

namespace SportManagementSystem.Modules.Services.Application.Commands.CreateSportService;

public record CreateSportServiceMessage(CreateSportServiceRequest Request) : IMessage<MbResult<long>>;