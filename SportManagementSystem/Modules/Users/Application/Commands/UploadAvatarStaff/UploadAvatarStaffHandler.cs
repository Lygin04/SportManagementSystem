using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Images.Application.Commands.DeleteImage;
using SportManagementSystem.Modules.Images.Application.Commands.UploadImage;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Users.Application.Commands.UploadAvatarStaff;

public class UploadAvatarStaffHandler(
    IStaffRepository staffRepository,
    IMediator mediator) : IMessageHandler<UploadAvatarStaffMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(UploadAvatarStaffMessage request, CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (staff == null)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Staff not found",
                status: StatusCodes.Status404NotFound,
                detail: "Сотрудник не найден."));
        }

        if (staff.Avatar != null)
        {
            await mediator.Send(new DeleteImageMessage(staff.AvatarId!.Value),  cancellationToken);
        }
        
        var imageId = await mediator.Send(new UploadImageMessage(request.Request), cancellationToken);
        staff.AvatarId = imageId.Data;

        await staffRepository.SetImageIdAsync(request.UserId, staff.AvatarId, cancellationToken);
        
        return MbResult<Unit>.Success(Unit.Value);
    }
}