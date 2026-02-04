using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Entities.Enums;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Repositories;
using SportManagementSystem.Modules.Scheduling.Domain.Repositories;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Clients.Application.Commands.CreateBooking;

public class CreateBookingHandler(
    IBookingRepository bookingRepository,
    IClientRepository clientRepository,
    ITrainingSessionRepository trainingSessionRepository) : IMessageHandler<CreateBookingMessage, MbResult<long>>
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

        var sessionExists = await trainingSessionRepository.ExistsAsync(request.Request.SessionId, cancellationToken);
        if (!sessionExists)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Training session not found",
                status: StatusCodes.Status409Conflict,
                detail: "Занятие не найдено."));
        }
        
        var booking = new DbBooking
        {
            ClientId = request.ClientId,
            SessionId = request.Request.SessionId,
            Booked = request.Request.Booked,
            Status = EBookingStatus.Booked
        };

        var result = await bookingRepository.CreateAsync(booking, cancellationToken);
        return MbResult<long>.Success(result.Id);
    }
}