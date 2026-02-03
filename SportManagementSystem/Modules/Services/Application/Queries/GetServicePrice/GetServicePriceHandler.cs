using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Repositories;

namespace SportManagementSystem.Modules.Services.Application.Queries.GetServicePrice;

public class GetServicePriceHandler(
    IServicePriceRepository servicePriceRepository) : IMessageHandler<GetServicePriceMessage, MbResult<DbServicePrice>>
{
    public async Task<MbResult<DbServicePrice>> Handle(GetServicePriceMessage request, CancellationToken cancellationToken)
    {
        var result = await servicePriceRepository.GetByIdAsync(request.Id, cancellationToken);

        if (result == null)
        {
            return MbResult<DbServicePrice>.Failure(new MbError(
                title: "Service Price not found",
                status: StatusCodes.Status404NotFound,
                detail: "Цена не найдена"));
        }
        
        return MbResult<DbServicePrice>.Success(result);
    }
}