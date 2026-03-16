using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Contracts.Requests;

namespace SportManagementSystem.Modules.Users.Application.Commands.RegisterBranchStaff;

public record RegisterBranchStaffMessage(long BranchId, RegisterStaffRequest Request) : IMessage<MbResult<Unit>>;
