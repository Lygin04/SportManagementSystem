using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Contracts.Response;
using SportManagementSystem.Modules.Clients.Domain.Repositories;

namespace SportManagementSystem.Modules.Clients.Application.Queries.GetMyBookings;

public class GetMyBookingsHandler(IBookingRepository bookingRepository)
    : IMessageHandler<GetMyBookingsMessage, MbResult<List<ClientBookingResponse>>>
{
    public async Task<MbResult<List<ClientBookingResponse>>> Handle(
        GetMyBookingsMessage request,
        CancellationToken cancellationToken)
    {
        var bookings = await bookingRepository.GetByClientIdAsync(request.ClientId, cancellationToken);

        var response = bookings
            .Select(booking => new ClientBookingResponse
            {
                Id = booking.Id,
                SessionId = booking.SessionId,
                SportServiceId = booking.Session?.SportServiceId ?? 0,
                SportServiceName = booking.Session?.SportService?.Name ?? string.Empty,
                BranchId = booking.Session?.SportService?.BranchId ?? 0,
                BranchName = booking.Session?.SportService?.Branch?.Name ?? string.Empty,
                TimeZoneId = booking.TimeZoneId,
                Booked = booking.Booked,
                SessionStartedDate = booking.Session?.StartedDate ?? default,
                SessionEndedDate = booking.Session?.EndedDate ?? default,
                Status = booking.Status.ToString()
            })
            .ToList();

        return MbResult<List<ClientBookingResponse>>.Success(response);
    }
}
