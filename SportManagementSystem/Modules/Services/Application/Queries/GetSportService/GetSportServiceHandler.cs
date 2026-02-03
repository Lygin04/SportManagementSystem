using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Repositories;

namespace SportManagementSystem.Modules.Services.Application.Queries.GetSportService;

public class GetSportServiceHandler(
    ISportServiceRepository sportServiceRepository) : IMessageHandler<GetSportServiceMessage, MbResult<DbSportService>>
{
    public async Task<MbResult<DbSportService>> Handle(GetSportServiceMessage request, CancellationToken cancellationToken)
    {
        var sportService = await sportServiceRepository.GetByIdAsync(request.Id, cancellationToken);

        if (sportService == null)
        {
            return MbResult<DbSportService>.Failure(new MbError(
                title: "Sport service not found",
                status: StatusCodes.Status404NotFound,
                detail: "Услуга не найдена"));
        }
        
        return MbResult<DbSportService>.Success(sportService);
    }
}