using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Enums;
using SportManagementSystem.Modules.Clients.Domain.Repositories;
using SportManagementSystem.Modules.Services.Domain.Repositories;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Clients.Application.Commands.CreateMembership;

public class CreateMembershipHandler(
    IMembershipRepository membershipRepository,
    IClientRepository clientRepository,
    ISportServiceRepository sportServiceRepository,
    IMembershipTemplateRepository membershipTemplateRepository) : IMessageHandler<CreateMembershipMessage, MbResult<long>>
{
    public async Task<MbResult<long>> Handle(CreateMembershipMessage request, CancellationToken cancellationToken)
    {
        var clientIdResult = ResolveClientId(request);
        if (!clientIdResult.IsSuccess)
        {
            return MbResult<long>.Failure(clientIdResult.Error!);
        }

        var clientId = clientIdResult.Data;

        var clientExists = await clientRepository.ExistsAsync(clientId, cancellationToken);
        if (!clientExists)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Client not found",
                status: StatusCodes.Status409Conflict,
                detail: "Клиента не существует"));
        }

        var membershipTemplate = await ResolveTemplateAsync(request, cancellationToken);
        if (!membershipTemplate.IsSuccess)
        {
            return MbResult<long>.Failure(membershipTemplate.Error!);
        }

        var sportServiceId = membershipTemplate.Data?.SportServiceId ?? request.Request.SportServiceId;
        var sportServiceExists =
            await sportServiceRepository.ExistsAsync(sportServiceId, cancellationToken);
        if (!sportServiceExists)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Sport service not found",
                status: StatusCodes.Status409Conflict,
                detail: "Спортивная секция не найдена"));
        }

        var startDate = request.Request.StartDate;
        var endDate = membershipTemplate.Data is null
            ? request.Request.EndDate
            : request.Request.StartDate.AddDays(Math.Max(0, membershipTemplate.Data.DurationDays - 1));

        var membership = new DbMembership
        {
            ClientId = clientId,
            MembershipTemplateId = membershipTemplate.Data?.Id,
            SportServiceId = sportServiceId,
            StartDate = startDate,
            EndDate = endDate,
            TotalVisits = membershipTemplate.Data?.VisitLimit ?? request.Request.RemainingVisits ?? 0,
            RemainingVisits = membershipTemplate.Data?.VisitLimit ?? request.Request.RemainingVisits,
            Status = EMembershipStatus.Active
        };
        var result = await membershipRepository.CreateAsync(membership, cancellationToken);
        return MbResult<long>.Success(result.Id);
    }

    private async Task<MbResult<DbMembershipTemplate?>> ResolveTemplateAsync(
        CreateMembershipMessage request,
        CancellationToken cancellationToken)
    {
        if (!request.Request.MembershipTemplateId.HasValue)
        {
            return MbResult<DbMembershipTemplate?>.Success(null);
        }

        var template = await membershipTemplateRepository.GetByIdAsync(request.Request.MembershipTemplateId.Value, cancellationToken);
        if (template is null || !template.IsActive)
        {
            return MbResult<DbMembershipTemplate?>.Failure(new MbError(
                title: "Membership template not found",
                status: StatusCodes.Status404NotFound,
                detail: "Шаблон абонемента не найден"));
        }

        return MbResult<DbMembershipTemplate?>.Success(template);
    }

    private static MbResult<long> ResolveClientId(CreateMembershipMessage request)
    {
        if (string.Equals(request.Role, "Client", StringComparison.OrdinalIgnoreCase))
        {
            return MbResult<long>.Success(request.UserId);
        }

        if (string.Equals(request.Role, "Manager", StringComparison.OrdinalIgnoreCase))
        {
            if (request.Request.ClientId is > 0)
            {
                return MbResult<long>.Success(request.Request.ClientId.Value);
            }

            return MbResult<long>.Failure(new MbError(
                title: "Client id is required",
                status: StatusCodes.Status400BadRequest,
                detail: "Для менеджера требуется идентификатор клиента"));
        }

        return MbResult<long>.Failure(new MbError(
            title: "Forbidden",
            status: StatusCodes.Status403Forbidden,
            detail: "Создание абонементов доступно только клиенту или менеджеру"));
    }
}
