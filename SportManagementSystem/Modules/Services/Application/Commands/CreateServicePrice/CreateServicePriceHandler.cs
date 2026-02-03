using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Repositories;

namespace SportManagementSystem.Modules.Services.Application.Commands.CreateServicePrice;

public class CreateServicePriceHandler(
    IServicePriceRepository servicePriceRepository,
    ISportServiceRepository sportServiceRepository) : IMessageHandler<CreateServicePriceMessage, MbResult<long>>
{
    public async Task<MbResult<long>> Handle(CreateServicePriceMessage request, CancellationToken cancellationToken)
    {
        var exists = await sportServiceRepository.ExistsAsync(request.Request.SportServiceId, cancellationToken);
        if (!exists)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Service Price not found",
                status: StatusCodes.Status409Conflict,
                detail: "Спортивная услуга не существует."));
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