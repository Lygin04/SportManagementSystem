using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Contracts.Response;

namespace SportManagementSystem.Modules.Users.Application.Queries.GetClientLookup;

public record GetClientLookupMessage(long ClientId) : IMessage<MbResult<GetUserResponse>>;
