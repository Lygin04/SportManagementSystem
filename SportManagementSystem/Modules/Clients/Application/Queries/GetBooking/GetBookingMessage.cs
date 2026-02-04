using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Domain.Entities;

namespace SportManagementSystem.Modules.Clients.Application.Queries.GetBooking;

public record GetBookingMessage(long Id) : IMessage<MbResult<DbBooking>>; 