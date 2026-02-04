using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Entities.Enums;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Repositories;
using SportManagementSystem.Modules.Services.Domain.Repositories;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Clients.Application.Commands.CreateMembership;

public class CreateMembershipHandler(
    IMembershipRepository membershipRepository,
    IClientRepository clientRepository,
    ISportServiceRepository sportServiceRepository) : IMessageHandler<CreateMembershipMessage, MbResult<long>>
{
    public async Task<MbResult<long>> Handle(CreateMembershipMessage request, CancellationToken cancellationToken)
    {
        var clientExists = await clientRepository.ExistsAsync(request.ClientId, cancellationToken);
        if (!clientExists)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Client not found",
                status: StatusCodes.Status409Conflict,
                detail: "Клиента не существует"));
        }

        var sportServiceExists =
            await sportServiceRepository.ExistsAsync(request.Request.SportServiceId, cancellationToken);
        if (!sportServiceExists)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Sport service not found",
                status: StatusCodes.Status409Conflict,
                detail: "Спортивная секция не найдена"));
        }

        var membership = new DbMembership
        {
            ClientId = request.ClientId,
            SportServiceId = request.Request.SportServiceId,
            StartDate = request.Request.StartDate,
            EndDate = request.Request.EndDate,
            TotalVisits = 0,
            RemainingVisits = request.Request.RemainingVisits,
            Status = EMembershipStatus.Active
        };
        var result = await membershipRepository.CreateAsync(membership, cancellationToken);
        return MbResult<long>.Success(result.Id);
    }
}