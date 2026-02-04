using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Scheduling.Contracts.Requests;

namespace SportManagementSystem.Modules.Scheduling.Application.Commands.CreateTrainingSession;

public record CreateTrainingSessionMessage(CreateTrainingSessionRequest Request) : IMessage<MbResult<long>>;