using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Contracts.Requests;

namespace SportManagementSystem.Modules.Users.Application.Commands.RegisterClient;

public record RegisterClientMessage(RegisterClientRequest Request) : IMessage<MbResult<Unit>>;