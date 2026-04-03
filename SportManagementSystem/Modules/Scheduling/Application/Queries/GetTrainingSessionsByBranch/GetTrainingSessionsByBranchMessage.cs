using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;

namespace SportManagementSystem.Modules.Scheduling.Application.Queries.GetTrainingSessionsByBranch;

public record GetTrainingSessionsByBranchMessage(long BranchId) : IMessage<MbResult<List<DbTrainingSession>>>;
