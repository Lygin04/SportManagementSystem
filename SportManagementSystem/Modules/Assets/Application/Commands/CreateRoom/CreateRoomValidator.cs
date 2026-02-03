using FluentValidation;

namespace SportManagementSystem.Modules.Assets.Application.Commands.CreateRoom;

public class CreateRoomValidator : AbstractValidator<CreateRoomMessage>
{
    public CreateRoomValidator()
    {
        RuleFor(x => x.Request.BranchId)
            .NotEmpty();

        RuleFor(x => x.Request.Name)
            .NotEmpty()
            .MinimumLength(4);
        
        RuleFor(x => x.Request.Capacity)
            .GreaterThan(0);
    }
}