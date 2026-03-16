using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Users.Contracts.Response;

namespace SportManagementSystem.Modules.Users.Application.Queries.GetBranchStaffs;

public record GetBranchStaffsMessage(long BranchId) : IMessage<MbResult<List<BranchStaffResponse>>>;
