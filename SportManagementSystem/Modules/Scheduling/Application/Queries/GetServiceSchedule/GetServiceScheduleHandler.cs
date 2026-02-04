using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Repositories;

namespace SportManagementSystem.Modules.Scheduling.Application.Queries.GetServiceSchedule;

public class GetServiceScheduleHandler(
    IServiceScheduleRepository serviceScheduleRepository) : IMessageHandler<GetServiceScheduleMessage, MbResult<DbServiceSchedule>>
{
    public async Task<MbResult<DbServiceSchedule>> Handle(GetServiceScheduleMessage request, CancellationToken cancellationToken)
    {
        var serviceSchedule = await serviceScheduleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (serviceSchedule is null)
        {
            return MbResult<DbServiceSchedule>.Failure(new MbError(
                title: "Service schedule not found",
                status: StatusCodes.Status404NotFound,
                detail: "Расписание тренировки не найдено."));
        }
        
        return MbResult<DbServiceSchedule>.Success(serviceSchedule);
    }
}