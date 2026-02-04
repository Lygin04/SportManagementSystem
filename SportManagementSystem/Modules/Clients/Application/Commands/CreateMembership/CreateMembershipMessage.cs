using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Contracts.Requests;

namespace SportManagementSystem.Modules.Clients.Application.Commands.CreateMembership;

public record CreateMembershipMessage(long ClientId, CreateMembershipRequest Request) : IMessage<MbResult<long>>;