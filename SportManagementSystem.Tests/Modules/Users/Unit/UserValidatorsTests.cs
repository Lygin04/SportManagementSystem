using SportManagementSystem.Modules.Users.Application.Commands.LoginUser;
using SportManagementSystem.Modules.Users.Application.Commands.RegisterClient;
using SportManagementSystem.Modules.Users.Application.Commands.RegisterStaff;
using SportManagementSystem.Modules.Users.Contracts.Requests;
using SportManagementSystem.Modules.Users.Domain.Enums;

namespace SportManagementSystem.Tests.Modules.Users.Unit;

public class UserValidatorsTests
{
    [Fact]
    public void RegisterClientValidator_WhenRequestInvalid_ReturnsErrors()
    {
        var validator = new RegisterClientValidator();
        var message = new RegisterClientMessage(new RegisterClientRequest
        {
            Email = "bad-email",
            Password = "short",
            FirstName = "",
            LastName = "",
            BirthDate = default,
            Phone = "123"
        });

        var result = validator.Validate(message);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Email");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Password");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.FirstName");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.LastName");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.BirthDate");
        Assert.Contains(result.Errors, e => e.PropertyName == "Request.Phone");
    }

    [Fact]
    public void RegisterStaffValidator_WhenRequestValid_HasNoErrors()
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
    public void LoginUserValidator_WhenRequestInvalid_ReturnsErrors()
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
}
