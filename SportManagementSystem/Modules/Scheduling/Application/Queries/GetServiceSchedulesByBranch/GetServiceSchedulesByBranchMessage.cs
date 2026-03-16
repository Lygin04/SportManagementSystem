using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;

namespace SportManagementSystem.Modules.Scheduling.Application.Queries.GetServiceSchedulesByBranch;

public record GetServiceSchedulesByBranchMessage(long BranchId) : IMessage<MbResult<List<DbServiceSchedule>>>;
