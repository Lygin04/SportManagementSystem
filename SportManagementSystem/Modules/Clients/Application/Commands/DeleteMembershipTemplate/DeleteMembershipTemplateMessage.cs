using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;

namespace SportManagementSystem.Modules.Clients.Application.Commands.DeleteMembershipTemplate;

public record DeleteMembershipTemplateMessage(long Id) : IMessage<MbResult<Unit>>;
