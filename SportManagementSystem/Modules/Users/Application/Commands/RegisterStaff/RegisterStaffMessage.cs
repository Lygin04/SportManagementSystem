using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Contracts.Requests;

namespace SportManagementSystem.Modules.Users.Application.Commands.RegisterStaff;

public record RegisterStaffMessage(RegisterStaffRequest Request) : IMessage<MbResult<Unit>>;