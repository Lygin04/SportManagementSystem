using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;

namespace SportManagementSystem.Modules.Scheduling.Application.Commands.DeleteServiceSchedule;

public record DeleteServiceScheduleMessage(long Id) : IMessage<MbResult<Unit>>;
