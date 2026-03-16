using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Services.Domain.Repositories;

namespace SportManagementSystem.Modules.Services.Application.Commands.DeleteSportService;

public class DeleteSportServiceHandler(
    ISportServiceRepository serviceRepository) : IMessageHandler<DeleteSportServiceMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(DeleteSportServiceMessage request, CancellationToken cancellationToken)
    {
        var sportService = await serviceRepository.ExistsAsync(request.Id, cancellationToken);
        if (!sportService)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Not Fount Sport Service",
                status: StatusCodes.Status404NotFound,
                detail: "Спорт. услуга не найдена."));
        }
        
        await serviceRepository.UpdateActive(request.Id, false, cancellationToken);
        return MbResult<Unit>.Success(Unit.Value);
    }
}