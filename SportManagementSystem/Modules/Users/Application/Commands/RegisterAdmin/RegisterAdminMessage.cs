using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Contracts.Requests;

namespace SportManagementSystem.Modules.Users.Application.Commands.RegisterAdmin;

public record RegisterAdminMessage(RegisterAdminRequest Request) : IMessage<MbResult<Unit>>;