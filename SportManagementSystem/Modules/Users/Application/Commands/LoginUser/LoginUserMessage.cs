using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Contracts.Requests;
using SportManagementSystem.Modules.Users.Contracts.Response;

namespace SportManagementSystem.Modules.Users.Application.Commands.LoginUser;

public record LoginUserMessage(LoginUserRequest Request) : IMessage<MbResult<LoginUserResponse>>;