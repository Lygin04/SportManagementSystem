using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Scheduling.Domain.Entities;
using SportManagementSystem.Modules.Scheduling.Domain.Repositories;
using SportManagementSystem.Modules.Services.Domain.Repositories;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Scheduling.Application.Commands.CreateServiceSchedule;

public class CreateServiceScheduleHandler(
    IServiceScheduleRepository serviceScheduleRepository,
    ISportServiceRepository sportServiceRepository,
    IStaffRepository staffRepository,
    IRoomRepository roomRepository) : IMessageHandler<CreateServiceScheduleMessage, MbResult<long>>
{
    public async Task<MbResult<long>> Handle(CreateServiceScheduleMessage request, CancellationToken cancellationToken)
    {
        var sportServiceExists =
            await sportServiceRepository.ExistsAsync(request.Request.SportServiceId, cancellationToken);
        if (!sportServiceExists)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Service schedule not found",
                status: StatusCodes.Status409Conflict,
                detail: "Спортивная услуга не найдена."));
        }

        if (request.Request.StaffId != null)
        {
            var staffExists = await staffRepository.ExistsAsync(request.Request.StaffId.Value, cancellationToken);
            if (!staffExists)
            {
                return MbResult<long>.Failure(new MbError(
                    title: "Staff not found",
                    status: StatusCodes.Status409Conflict,
                    detail: "Сотрудник не найден."));
            }
        }
        
        var roomExists = await roomRepository.ExistsAsync(request.Request.RoomId, cancellationToken);
        if (!roomExists)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Room not found",
                status: StatusCodes.Status409Conflict,
                detail: "Помещение не найдено."));
        }

        var serviceSchedule = new DbServiceSchedule
        {
            SportServiceId = request.Request.SportServiceId,
            StaffId = request.Request.StaffId,
            RoomId = request.Request.RoomId,
            DayOfWeek = request.Request.DayOfWeek,
            StartTime = request.Request.StartTime,
            EndTime = request.Request.EndTime,
            Note = request.Request.Note
        };

        var result = await serviceScheduleRepository.CreateAsync(serviceSchedule, cancellationToken);
        return MbResult<long>.Success(result.Id);
    }
}