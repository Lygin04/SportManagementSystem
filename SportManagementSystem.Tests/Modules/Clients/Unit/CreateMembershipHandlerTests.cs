using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using SportManagementSystem.BuildingBlocks.Time;
using SportManagementSystem.Modules.Analytics.Domain.Events;
using SportManagementSystem.Modules.Clients.Application.Commands.CreateMembership;
using SportManagementSystem.Modules.Clients.Contracts.Requests;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Enums;
using SportManagementSystem.Modules.Clients.Domain.Repositories;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Repositories;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Tests.Modules.Clients.Unit;

public class CreateMembershipHandlerTests
{
    private readonly Mock<IMembershipRepository> _membershipRepoMock;
    private readonly Mock<IClientRepository> _clientRepoMock;
    private readonly Mock<ISportServiceRepository> _sportServiceRepoMock;
    private readonly Mock<IMembershipTemplateRepository> _membershipTemplateRepoMock;
    private readonly Mock<IAppClock> _clockMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly CreateMembershipHandler _handler;

    public CreateMembershipHandlerTests()
    {
        _membershipRepoMock = new Mock<IMembershipRepository>();
        _clientRepoMock = new Mock<IClientRepository>();
        _sportServiceRepoMock = new Mock<ISportServiceRepository>();
        _membershipTemplateRepoMock = new Mock<IMembershipTemplateRepository>();
        _clockMock = new Mock<IAppClock>();
        _mediatorMock = new Mock<IMediator>();
        _clockMock.SetupGet(x => x.TodayInDefaultTimeZone).Returns(new DateOnly(2026, 4, 2));
        _clockMock.SetupGet(x => x.UtcNow).Returns(new DateTimeOffset(2026, 4, 2, 10, 0, 0, TimeSpan.Zero));
        _handler = new CreateMembershipHandler(
            _mediatorMock.Object,
            _membershipRepoMock.Object,
            _clientRepoMock.Object,
            _sportServiceRepoMock.Object,
            _membershipTemplateRepoMock.Object,
            _clockMock.Object);
    }

    [Fact]
    public async Task Handle_WhenClientDoesNotExist_ReturnsConflict()
    {
        _clientRepoMock.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _sportServiceRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DbSportService { Id = 5, BranchId = 9, Code = "svc", Name = "Service" });

        var result = await _handler.Handle(CreateMessage(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status409Conflict, result.Error!.Status);
        _membershipRepoMock.Verify(x => x.CreateAsync(It.IsAny<DbMembership>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenSportServiceDoesNotExist_ReturnsConflict()
    {
        _clientRepoMock.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _sportServiceRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((DbSportService?)null);

        var result = await _handler.Handle(CreateMessage(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Error);
        Assert.Equal(StatusCodes.Status409Conflict, result.Error!.Status);
        _membershipRepoMock.Verify(x => x.CreateAsync(It.IsAny<DbMembership>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenInputIsValid_CreatesMembershipWithDefaultValues()
    {
        var message = CreateMessage();
        DbMembership? createdMembership = null;
        _clientRepoMock.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _sportServiceRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DbSportService { Id = 5, BranchId = 9, Code = "svc", Name = "Service" });
        _membershipRepoMock
            .Setup(x => x.CreateAsync(It.IsAny<DbMembership>(), It.IsAny<CancellationToken>()))
            .Callback<DbMembership, CancellationToken>((entity, _) => createdMembership = entity)
            .ReturnsAsync((DbMembership entity, CancellationToken _) =>
            {
                entity.Id = 777;
                return entity;
            });

        var result = await _handler.Handle(message, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(777, result.Data);
        _membershipRepoMock.Verify(x => x.CreateAsync(It.IsAny<DbMembership>(), It.IsAny<CancellationToken>()), Times.Once);
        _mediatorMock.Verify(x => x.Publish(It.IsAny<ClientPurchasedMembershipDomainEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(createdMembership);
        Assert.Equal(0, createdMembership!.TotalVisits);
        Assert.Equal(EMembershipStatus.Active, createdMembership.Status);
        Assert.Equal(message.UserId, createdMembership.ClientId);
        Assert.Equal(message.Request.SportServiceId, createdMembership.SportServiceId);
    }

    private static CreateMembershipMessage CreateMessage()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return new CreateMembershipMessage(
            UserId: 12,
            Role: "Client",
            Request: new CreateMembershipRequest
            {
                SportServiceId = 5,
                StartDate = today.AddDays(2),
                EndDate = today.AddDays(32),
                RemainingVisits = 10
            });
    }
}
