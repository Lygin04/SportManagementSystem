using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Repositories;

namespace SportManagementSystem.Modules.Services.Application.Commands.CreateSportService;

public class CreateSportServiceHandler(
    ISportServiceRepository sportServiceRepository) : IMessageHandler<CreateSportServiceMessage, MbResult<long>>
{
    public async Task<MbResult<long>> Handle(CreateSportServiceMessage request, CancellationToken cancellationToken)
    {
        var sportService = new DbSportService
        {
            Name = request.Request.Name,
            Description = request.Request.Description,
            Category = request.Request.Category,
            IsActive = true,
            Created = DateTime.UtcNow
        };

        var result = await sportServiceRepository.CreateAsync(sportService, cancellationToken);
        return MbResult<long>.Success(result.Id);
    }
}