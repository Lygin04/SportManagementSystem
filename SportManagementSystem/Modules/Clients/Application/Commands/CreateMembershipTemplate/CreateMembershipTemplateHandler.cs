using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.BuildingBlocks.Time;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Repositories;
using SportManagementSystem.Modules.Services.Domain.Repositories;

namespace SportManagementSystem.Modules.Clients.Application.Commands.CreateMembershipTemplate;

public class CreateMembershipTemplateHandler(
    IAppClock clock,
    IMembershipTemplateRepository membershipTemplateRepository,
    IBranchRepository branchRepository,
    ISportServiceRepository sportServiceRepository,
    IServicePriceRepository servicePriceRepository) : IMessageHandler<CreateMembershipTemplateMessage, MbResult<long>>
{
    public async Task<MbResult<long>> Handle(CreateMembershipTemplateMessage request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.GetByIdAsync(request.Request.BranchId, cancellationToken);
        if (branch is null)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Branch not found",
                status: StatusCodes.Status404NotFound,
                detail: "Филиал не найден"));
        }

        var hasAccess = await branchRepository.HasManagementAccessAsync(request.Request.BranchId, request.UserId, cancellationToken);
        if (!hasAccess)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Forbidden",
                status: StatusCodes.Status403Forbidden,
                detail: "У пользователя нет доступа к управлению этим филиалом"));
        }

        var service = await sportServiceRepository.GetByIdAsync(request.Request.SportServiceId, cancellationToken);
        if (service is null || service.BranchId != request.Request.BranchId)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Sport service not found",
                status: StatusCodes.Status404NotFound,
                detail: "Услуга не найдена в выбранном филиале"));
        }

        var servicePrice = await servicePriceRepository.GetByIdAsync(request.Request.ServicePriceId, cancellationToken);
        if (servicePrice is null || servicePrice.SportServiceId != request.Request.SportServiceId)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Service price not found",
                status: StatusCodes.Status404NotFound,
                detail: "Цена услуги не найдена или не относится к выбранной услуге"));
        }

        var template = new DbMembershipTemplate
        {
            BranchId = request.Request.BranchId,
            SportServiceId = request.Request.SportServiceId,
            ServicePriceId = request.Request.ServicePriceId,
            Name = request.Request.Name.Trim(),
            Description = request.Request.Description?.Trim(),
            DurationDays = request.Request.DurationDays,
            VisitLimit = request.Request.VisitLimit,
            IsActive = true,
            Created = clock.UtcNow,
        };

        var created = await membershipTemplateRepository.CreateAsync(template, cancellationToken);
        return MbResult<long>.Success(created.Id);
    }
}
