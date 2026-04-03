using Moq;
using SportManagementSystem.BuildingBlocks.Time;
using SportManagementSystem.Modules.Clients.Application.Commands.CreateBooking;
using SportManagementSystem.Modules.Clients.Application.Commands.CreateMembership;
using SportManagementSystem.Modules.Clients.Contracts.Requests;

namespace SportManagementSystem.Tests.Modules.Clients.Unit;

public class ClientsValidatorsTests
{
    private static readonly DateTimeOffset FixedUtcNow = new(2026, 3, 27, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void CreateBookingValidator_WhenRequestInvalid_ReturnsErrors()
    {
        var validator = new CreateBookingValidator(CreateClock().Object);
        var message = new CreateBookingMessage(0, new CreateBookingRequest
        {
            SessionId = 0,
            Booked = FixedUtcNow.AddMinutes(-5)
        });

        var result = validator.Validate(message);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ClientId");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.SessionId");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Booked");
    }

    [Fact]
    public void CreateBookingValidator_WhenRequestValid_HasNoErrors()
    {
        var validator = new CreateBookingValidator(CreateClock().Object);
        var message = new CreateBookingMessage(1, new CreateBookingRequest
        {
            SessionId = 5,
            Booked = FixedUtcNow.AddMinutes(15)
        });

        var result = validator.Validate(message);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void CreateMembershipValidator_WhenRequestInvalid_ReturnsErrors()
    {
        var validator = new CreateMembershipValidator(CreateClock().Object);
        var today = new DateOnly(2026, 3, 27);
        var message = new CreateMembershipMessage(1, "Client", new CreateMembershipRequest
        {
            SportServiceId = 0,
            StartDate = today,
            EndDate = today.AddDays(1),
            RemainingVisits = 0
        });

        var result = validator.Validate(message);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.SportServiceId");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.StartDate");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.EndDate");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.RemainingVisits");
    }

    [Fact]
    public void CreateMembershipValidator_WhenRequestValid_HasNoErrors()
    {
        var validator = new CreateMembershipValidator(CreateClock().Object);
        var today = new DateOnly(2026, 3, 27);
        var message = new CreateMembershipMessage(1, "Client", new CreateMembershipRequest
        {
            SportServiceId = 5,
            StartDate = today.AddDays(1),
            EndDate = today.AddDays(3),
            RemainingVisits = 10
        });

        var result = validator.Validate(message);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void CreateMembershipValidator_WhenTemplateSelected_DoesNotRequireDates()
    {
        var validator = new CreateMembershipValidator(CreateClock().Object);
        var message = new CreateMembershipMessage(1, "Client", new CreateMembershipRequest
        {
            MembershipTemplateId = 5,
            SportServiceId = 0
        });

        var result = validator.Validate(message);

        Assert.True(result.IsValid);
    }

    private static Mock<IAppClock> CreateClock()
    {
        var clock = new Mock<IAppClock>();
        clock.SetupGet(x => x.UtcNow).Returns(FixedUtcNow);
        clock.SetupGet(x => x.TodayInDefaultTimeZone).Returns(new DateOnly(2026, 3, 27));
        return clock;
    }
}
