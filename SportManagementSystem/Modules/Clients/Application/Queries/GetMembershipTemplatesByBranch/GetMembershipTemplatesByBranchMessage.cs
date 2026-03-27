using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Contracts.Response;

namespace SportManagementSystem.Modules.Clients.Application.Queries.GetMembershipTemplatesByBranch;

public record GetMembershipTemplatesByBranchMessage(long BranchId) : IMessage<MbResult<List<MembershipTemplateResponse>>>;
