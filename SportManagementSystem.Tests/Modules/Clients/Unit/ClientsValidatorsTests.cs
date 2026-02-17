using SportManagementSystem.Modules.Clients.Application.Commands.CreateBooking;
using SportManagementSystem.Modules.Clients.Application.Commands.CreateMembership;
using SportManagementSystem.Modules.Clients.Contracts.Requests;

namespace SportManagementSystem.Tests.Modules.Clients.Unit;

public class ClientsValidatorsTests
{
    [Fact]
    public void CreateBookingValidator_WhenRequestInvalid_ReturnsErrors()
    {
        var validator = new CreateBookingValidator();
        var message = new CreateBookingMessage(0, new CreateBookingRequest
        {
            SessionId = 0,
            Booked = DateTime.UtcNow.AddMinutes(-5)
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
        var validator = new CreateBookingValidator();
        var message = new CreateBookingMessage(1, new CreateBookingRequest
        {
            SessionId = 5,
            Booked = DateTime.UtcNow.AddMinutes(15)
        });

        var result = validator.Validate(message);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void CreateMembershipValidator_WhenRequestInvalid_ReturnsErrors()
    {
        var validator = new CreateMembershipValidator();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var message = new CreateMembershipMessage(1, new CreateMembershipRequest
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
        var validator = new CreateMembershipValidator();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var message = new CreateMembershipMessage(1, new CreateMembershipRequest
        {
            SportServiceId = 5,
            StartDate = today.AddDays(1),
            EndDate = today.AddDays(2),
            RemainingVisits = 10
        });

        var result = validator.Validate(message);

        Assert.True(result.IsValid);
    }
}
