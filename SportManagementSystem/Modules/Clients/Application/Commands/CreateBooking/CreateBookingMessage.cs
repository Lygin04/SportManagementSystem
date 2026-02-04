using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Clients.Contracts.Requests;

namespace SportManagementSystem.Modules.Clients.Application.Commands.CreateBooking;

public record CreateBookingMessage(long ClientId, CreateBookingRequest Request) : IMessage<MbResult<long>>;