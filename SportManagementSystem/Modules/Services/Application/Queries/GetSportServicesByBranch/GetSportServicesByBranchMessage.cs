using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Services.Contracts.Response;

namespace SportManagementSystem.Modules.Services.Application.Queries.GetSportServicesByBranch;

public record GetSportServicesByBranchMessage(long BranchId) : IMessage<MbResult<List<SportServiceResponse>>>;
