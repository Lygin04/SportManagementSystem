using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.BuildingBlocks.Time;
using SportManagementSystem.Modules.Analytics.Contracts.Requests;
using SportManagementSystem.Modules.Analytics.Domain.Events;
using SportManagementSystem.Modules.Assets.Domain.Repositories;

namespace SportManagementSystem.Modules.Analytics.Application.Commands.TrackBranchInteraction;

public class TrackBranchInteractionHandler(
    IBranchRepository branchRepository,
    IMediator mediator,
    IAppClock clock) : IMessageHandler<TrackBranchInteractionMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(TrackBranchInteractionMessage request, CancellationToken cancellationToken)
    {
        var exists = await branchRepository.ExistsAsync(request.Request.BranchId, cancellationToken);
        if (!exists)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Branch not found",
                status: StatusCodes.Status404NotFound,
                detail: "Филиал не найден"));
        }

        var actionType = request.Request.ActionType.Trim().ToLowerInvariant();
        var role = string.IsNullOrWhiteSpace(request.Role) ? "Anonymous" : request.Role;
        var clientId = string.Equals(role, "Client", StringComparison.OrdinalIgnoreCase) ? request.UserId : null;

        switch (actionType)
        {
            case BranchInteractionActionTypes.CardViewed:
                await mediator.Publish(
                    new BranchCardViewedDomainEvent(request.Request.BranchId, clientId, role, clock.UtcNow),
                    cancellationToken);
                break;
            case BranchInteractionActionTypes.DetailsOpened:
                await mediator.Publish(
                    new BranchDetailsOpenedDomainEvent(request.Request.BranchId, clientId, role, clock.UtcNow),
                    cancellationToken);
                break;
            default:
                return MbResult<Unit>.Failure(new MbError(
                    title: "Unsupported action type",
                    status: StatusCodes.Status400BadRequest,
                    detail: $"Неизвестный тип события: {request.Request.ActionType}"));
        }

        return MbResult<Unit>.Success(Unit.Value);
    }
}
