using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Contracts.Response;

namespace SportManagementSystem.Modules.Users.Application.Queries.GetClient;

public record GetClientMessage(long Id) : IMessage<MbResult<GetUserResponse>>;