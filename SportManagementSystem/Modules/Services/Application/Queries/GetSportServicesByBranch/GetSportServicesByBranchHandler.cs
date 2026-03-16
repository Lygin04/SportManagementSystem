using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Services.Contracts.Response;
using SportManagementSystem.Modules.Services.Domain.Repositories;

namespace SportManagementSystem.Modules.Services.Application.Queries.GetSportServicesByBranch;

public class GetSportServicesByBranchHandler(
    ISportServiceRepository sportServiceRepository)
    : IMessageHandler<GetSportServicesByBranchMessage, MbResult<List<SportServiceResponse>>>
{
    public async Task<MbResult<List<SportServiceResponse>>> Handle(
        GetSportServicesByBranchMessage request,
        CancellationToken cancellationToken)
    {
        var services = await sportServiceRepository.GetByBranchAsync(request.BranchId, cancellationToken);
        var response = services
            .Select(service => new SportServiceResponse
            {
                Id = service.Id,
                BranchId = service.BranchId,
                Name = service.Name,
                Description = service.Description,
                Category = service.Category?.ToString(),
                Prices = service.Prices
                    .OrderByDescending(price => price.ValidFrom)
                    .Select(price => new ServicePriceResponse
                    {
                        Id = price.Id,
                        SportServiceId = price.SportServiceId,
                        Amount = price.Amount,
                        Currency = price.Currency.ToString(),
                        ValidFrom = price.ValidFrom,
                        ValidTo = price.ValidTo,
                    })
                    .ToList(),
            })
            .ToList();

        return MbResult<List<SportServiceResponse>>.Success(response);
    }
}
