using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Domain.Entities;

namespace SportManagementSystem.Modules.Users.Application.Queries.GetStaff;

public record GetStaffMessage(long Id) : IMessage<MbResult<DbStaff>>;