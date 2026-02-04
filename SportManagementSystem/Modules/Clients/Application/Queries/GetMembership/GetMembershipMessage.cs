using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Domain.Entities;

namespace SportManagementSystem.Modules.Clients.Application.Queries.GetMembership;

public record GetMembershipMessage(long Id) : IMessage<MbResult<DbMembership>>;