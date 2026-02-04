using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Repositories;

namespace SportManagementSystem.Modules.Clients.Application.Queries.GetBooking;

public class GetBookingHandler(IBookingRepository bookingRepository) : IMessageHandler<GetBookingMessage, MbResult<DbBooking>>
{
    public async Task<MbResult<DbBooking>> Handle(GetBookingMessage request, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetByIdAsync(request.Id, cancellationToken);

        if (booking == null)
        {
            return MbResult<DbBooking>.Failure(new MbError(
                title: "Booking not found",
                status: StatusCodes.Status404NotFound,
                detail: "Запись не найдена"));
        }
        
        return MbResult<DbBooking>.Success(booking);
    }
}