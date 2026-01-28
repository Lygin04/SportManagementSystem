using FluentValidation;

namespace SportManagementSystem.Modules.Users.Application.Commands.LoginUser;

public class LoginUserValidator : AbstractValidator<LoginUserMessage>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Request.Password)
            .NotEmpty()
            .Length(8, 100);
    }
}