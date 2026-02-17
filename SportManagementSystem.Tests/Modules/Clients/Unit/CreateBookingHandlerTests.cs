using Microsoft.AspNetCore.Http;
using Moq;
using SportManagementSystem.Modules.Clients.Application.Commands.CreateBooking;
using SportManagementSystem.Modules.Clients.Contracts.Requests;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Enums;
using SportManagementSystem.Modules.Clients.Domain.Repositories;
using SportManagementSystem.Modules.Scheduling.Domain.Repositories;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Tests.Modules.Clients.Unit;

public class CreateBookingHandlerTests
{
    private readonly Mock<IBookingRepository> _bookingRepository;
    private readonly Mock<IClientRepository> _clientRepository;
    private readonly Mock<ITrainingSessionRepository> _trainingSessionRepository;
    private readonly CreateBookingHandler _handler;

    public CreateBookingHandlerTests()
    {
        _bookingRepository = new Mock<IBookingRepository>();
        _clientRepository = new Mock<IClientRepository>();
        _trainingSessionRepository = new Mock<ITrainingSessionRepository>();
        _handler = new CreateBookingHandler(
            _bookingRepository.Object,
            _clientRepository.Object,
            _trainingSessionRepository.Object);
    }

    [Fact]
    public async Task Handle_WhenClientMissing_ReturnsConflict()
    {
        _clientRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _trainingSessionRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await _handler.Handle(CreateMessage(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status409Conflict, result.Error!.Status);
        _bookingRepository.Verify(x => x.CreateAsync(It.IsAny<DbBooking>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenSessionMissing_ReturnsConflict()
    {
        _clientRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _trainingSessionRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var result = await _handler.Handle(CreateMessage(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status409Conflict, result.Error!.Status);
        _bookingRepository.Verify(x => x.CreateAsync(It.IsAny<DbBooking>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRequestValid_CreatesBooking()
    {
        DbBooking? createdBooking = null;
        _clientRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _trainingSessionRepository.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _bookingRepository
            .Setup(x => x.CreateAsync(It.IsAny<DbBooking>(), It.IsAny<CancellationToken>()))
            .Callback<DbBooking, CancellationToken>((entity, _) => createdBooking = entity)
            .ReturnsAsync((DbBooking entity, CancellationToken _) =>
            {
                entity.Id = 123;
                return entity;
            });

        var result = await _handler.Handle(CreateMessage(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(123, result.Data);
        _bookingRepository.Verify(x => x.CreateAsync(It.IsAny<DbBooking>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(createdBooking);
        Assert.Equal(EBookingStatus.Booked, createdBooking!.Status);
    }

    private static CreateBookingMessage CreateMessage() =>
        new(
            ClientId: 10,
            Request: new CreateBookingRequest
            {
                SessionId = 99,
                Booked = DateTime.UtcNow.AddHours(2)
            });
}
