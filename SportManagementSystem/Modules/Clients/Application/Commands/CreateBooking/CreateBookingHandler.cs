using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.BuildingBlocks.Time;
using SportManagementSystem.Modules.Analytics.Domain.Events;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Enums;
using SportManagementSystem.Modules.Clients.Domain.Repositories;
using SportManagementSystem.Modules.Scheduling.Domain.Repositories;
using SportManagementSystem.Modules.Services.Domain.Repositories;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Clients.Application.Commands.CreateBooking;

public class CreateBookingHandler(
    IAppClock clock,
    IMediator mediator,
    IBookingRepository bookingRepository,
    IClientRepository clientRepository,
    ITrainingSessionRepository trainingSessionRepository,
    ISportServiceRepository sportServiceRepository) : IMessageHandler<CreateBookingMessage, MbResult<long>>
{
    public async Task<MbResult<long>> Handle(CreateBookingMessage request, CancellationToken cancellationToken)
    {
        var clientExists = await clientRepository.ExistsAsync(request.ClientId, cancellationToken);
        if (!clientExists)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Client not found",
                status: StatusCodes.Status409Conflict,
                detail: "Клиента не существует"));
        }

        var session = await trainingSessionRepository.GetByIdAsync(request.Request.SessionId, cancellationToken);
        if (session is null)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Training session not found",
                status: StatusCodes.Status409Conflict,
                detail: "Занятие не найдено."));
        }

        var sportService = await sportServiceRepository.GetByIdAsync(session.SportServiceId, cancellationToken);
        if (sportService is null)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Sport service not found",
                status: StatusCodes.Status409Conflict,
                detail: "Спортивная секция не найдена"));
        }

        var timeZoneId = string.IsNullOrWhiteSpace(request.Request.TimeZoneId)
            ? clock.DefaultTimeZone.Id
            : AppTimeZoneResolver.Resolve(request.Request.TimeZoneId).Id;

        var booking = new DbBooking
        {
            ClientId = request.ClientId,
            SessionId = request.Request.SessionId,
            TimeZoneId = timeZoneId,
            Booked = clock.UtcNow,
            Status = EBookingStatus.Booked
        };

        var result = await bookingRepository.CreateAsync(booking, cancellationToken);

        await mediator.Publish(new ClientBookedTrainingSessionDomainEvent(
            ClientId: request.ClientId,
            BookingId: result.Id,
            SessionId: session.Id,
            SportServiceId: session.SportServiceId,
            BranchId: sportService.BranchId,
            TimeZoneId: timeZoneId,
            OccurredOnUtc: booking.Booked), cancellationToken);

        return MbResult<long>.Success(result.Id);
    }
}
