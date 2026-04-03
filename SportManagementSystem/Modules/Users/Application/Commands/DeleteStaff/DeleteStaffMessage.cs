using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;

namespace SportManagementSystem.Modules.Users.Application.Commands.DeleteStaff;

public record DeleteStaffMessage(long StaffId, long CurrentUserId) : IMessage<MbResult<Unit>>;
