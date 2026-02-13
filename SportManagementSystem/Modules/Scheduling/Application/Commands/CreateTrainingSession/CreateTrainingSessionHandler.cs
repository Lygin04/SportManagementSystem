using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Enums;
using SportManagementSystem.Modules.Scheduling.Domain.Repositories;
using SportManagementSystem.Modules.Services.Domain.Repositories;
using SportManagementSystem.Modules.Users.Domain.Enums;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Scheduling.Application.Commands.CreateTrainingSession;

public class CreateTrainingSessionHandler(
    ITrainingSessionRepository trainingSessionRepository,
    ISportServiceRepository sportServiceRepository,
    IServiceScheduleRepository serviceScheduleRepository,
    IStaffRepository staffRepository,
    IUserAccountRepository userAccountRepository) : IMessageHandler<CreateTrainingSessionMessage, MbResult<long>>
{
    public async Task<MbResult<long>> Handle(CreateTrainingSessionMessage request, CancellationToken cancellationToken)
    {
        var sportServiceExists =
            await sportServiceRepository.ExistsAsync(request.Request.SportServiceId, cancellationToken);
        if (!sportServiceExists)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Sport service not found",
                status: StatusCodes.Status409Conflict,
                detail: "Спортивная услуга не найдена."));
        }

        if (request.Request.ScheduleId != null)
        {
            var scheduleExists =
                await serviceScheduleRepository.ExistsAsync(request.Request.ScheduleId.Value, cancellationToken);
            if (!scheduleExists)
            {
                return MbResult<long>.Failure(new MbError(
                    title: "Schedule not found",
                    status: StatusCodes.Status409Conflict,
                    detail: "Расписание не найдено."));
            }
        }
        
        var trainer = await staffRepository.GetByIdAsync(request.Request.TrainerId, cancellationToken);
        if (trainer == null)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Trainer not found",
                status: StatusCodes.Status404NotFound,
                detail: "Тренер не найден."));
        }
        var userAccount = await userAccountRepository.GetByIdAsync(request.Request.TrainerId, cancellationToken);
        if (userAccount!.Role != EUserRole.Trainer)
        {
            return MbResult<long>.Failure(new MbError(
                title: "The employee is not a trainer",
                status: StatusCodes.Status409Conflict,
                detail: "Сотрудник не является тренером"));
        }
        
        
        var trainingSession = new DbTrainingSession
        {
            SportServiceId = request.Request.SportServiceId,
            ScheduleId = request.Request.ScheduleId,
            StartedDate = request.Request.StartedDate,
            EndedDate = request.Request.EndedDate,
            TrainerId = request.Request.TrainerId,
            Status = ESessionStatus.Planned
        };

        var result = await trainingSessionRepository.CreateAsync(trainingSession, cancellationToken);
        return MbResult<long>.Success(result.Id);
    }
}