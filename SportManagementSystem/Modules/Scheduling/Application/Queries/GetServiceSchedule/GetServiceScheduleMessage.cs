using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;

namespace SportManagementSystem.Modules.Scheduling.Application.Queries.GetServiceSchedule;

public record GetServiceScheduleMessage(long Id) : IMessage<MbResult<DbServiceSchedule>>;