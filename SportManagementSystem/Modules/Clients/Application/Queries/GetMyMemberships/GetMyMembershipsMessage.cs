using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Contracts.Response;

namespace SportManagementSystem.Modules.Clients.Application.Queries.GetMyMemberships;

public record GetMyMembershipsMessage(long ClientId) : IMessage<MbResult<List<ClientMembershipResponse>>>;
