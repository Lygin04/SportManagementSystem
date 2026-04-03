using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;

namespace SportManagementSystem.Modules.Scheduling.Application.Commands.DeleteTrainingSession;

public record DeleteTrainingSessionMessage(long Id) : IMessage<MbResult<Unit>>;
