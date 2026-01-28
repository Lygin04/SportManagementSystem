using System.Globalization;
using FluentValidation;

namespace SportManagementSystem.Modules.Users.Application.Commands.RegisterStaff;

public class RegisterStaffValidator : AbstractValidator<RegisterStaffMessage>
{
    public RegisterStaffValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Request.Password)
            .NotEmpty()
            .Length(8, 100);

        RuleFor(x => x.Request.FirstName)
            .NotEmpty();
        
        RuleFor(x => x.Request.LastName)
            .NotEmpty();

        RuleFor(x => x.Request.BirthDate)
            .NotEmpty();
        
        RuleFor(x => x.Request.Phone)
            .NotEmpty()
            .Length(8, 15)
            .Matches(@"^\+?[1-9]\d{7,14}$");

        RuleFor(x => x.Request.Role)
            .NotEmpty()
            .IsInEnum();
    }
}