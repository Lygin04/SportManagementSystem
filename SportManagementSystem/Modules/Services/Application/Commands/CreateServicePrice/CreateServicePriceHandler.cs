using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Repositories;

namespace SportManagementSystem.Modules.Services.Application.Commands.CreateServicePrice;

public class CreateServicePriceHandler(
    IServicePriceRepository servicePriceRepository,
    ISportServiceRepository sportServiceRepository,
    IBranchRepository branchRepository) : IMessageHandler<CreateServicePriceMessage, MbResult<long>>
{
    public async Task<MbResult<long>> Handle(CreateServicePriceMessage request, CancellationToken cancellationToken)
    {
        var sportService = await sportServiceRepository.GetByIdAsync(request.Request.SportServiceId, cancellationToken);
        if (sportService is null)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Service Price not found",
                status: StatusCodes.Status409Conflict,
                detail: "Спортивная услуга не существует."));
        }

        if (string.Equals(request.Role, "Manager", StringComparison.OrdinalIgnoreCase))
        {
            var hasAccess = await branchRepository.HasManagementAccessAsync(sportService.BranchId, request.UserId, cancellationToken);
            if (!hasAccess)
            {
                return MbResult<long>.Failure(new MbError(
                    title: "Forbidden",
                    status: StatusCodes.Status403Forbidden,
                    detail: "У пользователя нет доступа к управлению этим филиалом"));
            }
        }
        
        var servicePrice = new DbServicePrice
        {
            SportServiceId = request.Request.SportServiceId,
            Amount = request.Request.Amount,
            Currency = request.Request.Currency,
            ValidFrom = request.Request.ValidFrom,
            ValidTo = request.Request.ValidTo
        };

        var result = await servicePriceRepository.CreateAsync(servicePrice, cancellationToken);
        return MbResult<long>.Success(result.Id);
    }
}
