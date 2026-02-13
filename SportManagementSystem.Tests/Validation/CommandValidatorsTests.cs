using SportManagementSystem.Modules.Clients.Application.Commands.CreateMembership;
using SportManagementSystem.Modules.Clients.Contracts.Requests;
using SportManagementSystem.Modules.Users.Application.Commands.LoginUser;
using SportManagementSystem.Modules.Users.Application.Commands.RegisterStaff;
using SportManagementSystem.Modules.Users.Contracts.Requests;
using SportManagementSystem.Modules.Users.Domain.Enums;

namespace SportManagementSystem.Tests.Validation;

public class CommandValidatorsTests
{
    [Fact]
    public void RegisterStaffValidator_WhenRequestIsValid_HasNoErrors()
    {
        var validator = new RegisterStaffValidator();
        var message = new RegisterStaffMessage(new RegisterStaffRequest
        {
            Email = "staff@example.com",
            Password = "Password123",
            FirstName = "Jane",
            LastName = "Doe",
            Patronymic = "A.",
            BirthDate = new DateOnly(1995, 5, 10),
            Phone = "+12345678901",
            Role = EUserRole.Manager
        });

        var result = validator.Validate(message);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void RegisterStaffValidator_WhenEmailPasswordPhoneInvalid_ReturnsErrors()
    {
        var validator = new RegisterStaffValidator();
        var message = new RegisterStaffMessage(new RegisterStaffRequest
        {
            Email = "invalid-email",
            Password = "123",
            FirstName = "Jane",
            LastName = "Doe",
            BirthDate = new DateOnly(1995, 5, 10),
            Phone = "123",
            Role = EUserRole.Manager
        });

        var result = validator.Validate(message);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Email");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Password");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Phone");
    }

    [Fact]
    public void LoginUserValidator_WhenRequestIsInvalid_ReturnsErrors()
    {
        var validator = new LoginUserValidator();
        var message = new LoginUserMessage(new LoginUserRequest
        {
            Email = "not-email",
            Password = "short"
        });

        var result = validator.Validate(message);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Email");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Password");
    }

    [Fact]
    public void CreateMembershipValidator_WhenDatesAndVisitsInvalid_ReturnsErrors()
    {
        var validator = new CreateMembershipValidator();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var message = new CreateMembershipMessage(1, new CreateMembershipRequest
        {
            SportServiceId = 1,
            StartDate = today,
            EndDate = today.AddDays(1),
            RemainingVisits = 0
        });

        var result = validator.Validate(message);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.StartDate");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.EndDate");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.RemainingVisits");
    }

    [Fact]
    public void CreateMembershipValidator_WhenRequestIsValid_HasNoErrors()
    {
        var validator = new CreateMembershipValidator();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var message = new CreateMembershipMessage(1, new CreateMembershipRequest
        {
            SportServiceId = 10,
            StartDate = today.AddDays(1),
            EndDate = today.AddDays(2),
            RemainingVisits = 15
        });

        var result = validator.Validate(message);

        Assert.True(result.IsValid);
    }
}
