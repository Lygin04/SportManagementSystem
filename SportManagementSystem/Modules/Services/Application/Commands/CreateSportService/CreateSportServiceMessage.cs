using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Services.Contracts.Requests;

namespace SportManagementSystem.Modules.Services.Application.Commands.CreateSportService;

public record CreateSportServiceMessage(long AdminId, CreateSportServiceRequest Request) : IMessage<MbResult<long>>;
