using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Scheduling.Contracts.Requests;

namespace SportManagementSystem.Modules.Scheduling.Application.Commands.CreateServiceSchedule;

public record CreateServiceScheduleMessage(CreateServiceScheduleRequest Request) : IMessage<MbResult<long>>;