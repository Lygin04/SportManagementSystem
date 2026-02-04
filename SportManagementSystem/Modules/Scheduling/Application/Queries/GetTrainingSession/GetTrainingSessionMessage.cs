using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;

namespace SportManagementSystem.Modules.Scheduling.Application.Queries.GetTrainingSession;

public record GetTrainingSessionMessage(long Id) : IMessage<MbResult<DbTrainingSession>>;