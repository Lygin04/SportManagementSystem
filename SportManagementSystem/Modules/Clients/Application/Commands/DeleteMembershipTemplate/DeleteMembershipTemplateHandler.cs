using MediatR;
using Microsoft.EntityFrameworkCore;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Data;

namespace SportManagementSystem.Modules.Clients.Application.Commands.DeleteMembershipTemplate;

public class DeleteMembershipTemplateHandler(ApplicationDbContext db)
    : IMessageHandler<DeleteMembershipTemplateMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(DeleteMembershipTemplateMessage request, CancellationToken cancellationToken)
    {
        var templateExists = await db.MembershipTemplates.AnyAsync(template => template.Id == request.Id, cancellationToken);
        if (!templateExists)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Membership template not found",
                status: StatusCodes.Status404NotFound,
                detail: "Абонемент не найден."));
        }

        await db.MembershipTemplates
            .Where(template => template.Id == request.Id)
            .ExecuteDeleteAsync(cancellationToken);

        db.ChangeTracker.Clear();

        return MbResult<Unit>.Success(Unit.Value);
    }
}
