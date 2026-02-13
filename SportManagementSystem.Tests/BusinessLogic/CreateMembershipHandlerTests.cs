using Microsoft.AspNetCore.Http;
using Moq;
using SportManagementSystem.Modules.Clients.Application.Commands.CreateMembership;
using SportManagementSystem.Modules.Clients.Contracts.Requests;
using SportManagementSystem.Modules.Clients.Domain.Entities;
using SportManagementSystem.Modules.Clients.Domain.Enums;
using SportManagementSystem.Modules.Clients.Domain.Repositories;
using SportManagementSystem.Modules.Services.Domain.Repositories;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Tests.BusinessLogic;

public class CreateMembershipHandlerTests
{
    private readonly Mock<IMembershipRepository> _membershipRepoMock;
    private readonly Mock<IClientRepository> _clientRepoMock;
    private readonly Mock<ISportServiceRepository> _sportServiceRepoMock;
    private readonly CreateMembershipHandler _handler;

    public CreateMembershipHandlerTests()
    {
        _membershipRepoMock = new Mock<IMembershipRepository>();
        _clientRepoMock = new Mock<IClientRepository>();
        _sportServiceRepoMock = new Mock<ISportServiceRepository>();
        _handler = new CreateMembershipHandler(_membershipRepoMock.Object, _clientRepoMock.Object, _sportServiceRepoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenClientDoesNotExist_ReturnsConflict()
    {
        _clientRepoMock.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _sportServiceRepoMock.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

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
        _sportServiceRepoMock.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

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
        _sportServiceRepoMock.Setup(x => x.ExistsAsync(It.IsAny<long>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
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
        Assert.NotNull(createdMembership);
        Assert.Equal(0, createdMembership!.TotalVisits);
        Assert.Equal(EMembershipStatus.Active, createdMembership.Status);
        Assert.Equal(message.ClientId, createdMembership.ClientId);
        Assert.Equal(message.Request.SportServiceId, createdMembership.SportServiceId);
    }

    private static CreateMembershipMessage CreateMessage()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return new CreateMembershipMessage(
            ClientId: 12,
            Request: new CreateMembershipRequest
            {
                SportServiceId = 5,
                StartDate = today.AddDays(2),
                EndDate = today.AddDays(32),
                RemainingVisits = 10
            });
    }
}
