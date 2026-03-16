using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Contracts.Response;

namespace SportManagementSystem.Modules.Users.Application.Queries.GetStaff;

public record GetStaffMessage(long Id) : IMessage<MbResult<GetUserResponse>>;