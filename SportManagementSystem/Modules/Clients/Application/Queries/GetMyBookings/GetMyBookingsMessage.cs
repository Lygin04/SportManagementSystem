using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Contracts.Response;

namespace SportManagementSystem.Modules.Clients.Application.Queries.GetMyBookings;

public record GetMyBookingsMessage(long ClientId) : IMessage<MbResult<List<ClientBookingResponse>>>;
