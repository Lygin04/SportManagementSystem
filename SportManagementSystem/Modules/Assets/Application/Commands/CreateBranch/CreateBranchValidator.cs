using FluentValidation;

namespace SportManagementSystem.Modules.Assets.Application.Commands.CreateBranch;

public class CreateBranchValidator : AbstractValidator<CreateBranchMessage>
{
    public CreateBranchValidator()
    {
        RuleFor(x => x.Request.Name)
            .NotEmpty();
        
        RuleFor(x => x.Request.Address)
            .NotEmpty();
    }
}