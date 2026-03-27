using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Contracts.Requests;

namespace SportManagementSystem.Modules.Clients.Application.Commands.CreateMembershipTemplate;

public record CreateMembershipTemplateMessage(long UserId, string Role, CreateMembershipTemplateRequest Request)
    : IMessage<MbResult<long>>;
