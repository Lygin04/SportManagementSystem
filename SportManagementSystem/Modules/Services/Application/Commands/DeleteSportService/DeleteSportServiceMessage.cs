using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;

namespace SportManagementSystem.Modules.Services.Application.Commands.DeleteSportService;

public record DeleteSportServiceMessage(long Id) : IMessage<MbResult<Unit>>;