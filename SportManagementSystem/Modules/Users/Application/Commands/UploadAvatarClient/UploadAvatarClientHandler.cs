using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Images.Application.Commands.DeleteImage;
using SportManagementSystem.Modules.Images.Application.Commands.UploadImage;
using SportManagementSystem.Modules.Users.Domain.Repositories;

namespace SportManagementSystem.Modules.Users.Application.Commands.UploadAvatarClient;

public class UploadAvatarClientHandler(
    IClientRepository clientRepository,
    IMediator mediator) : IMessageHandler<UploadAvatarClientMessage, MbResult<Unit>>
{
    public async Task<MbResult<Unit>> Handle(UploadAvatarClientMessage request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (client == null)
        {
            return MbResult<Unit>.Failure(new MbError(
                title: "Client not found",
                status: StatusCodes.Status404NotFound,
                detail: "Клиент не найден."));
        }

        if (client.Avatar != null)
        {
             await mediator.Send(new DeleteImageMessage(client.AvatarId!.Value),  cancellationToken);
        }
        
        var imageId = await mediator.Send(new UploadImageMessage(request.Request), cancellationToken);
        client.AvatarId = imageId.Data;
        client.Modified = DateTime.UtcNow;
        
        await clientRepository.SetImageIdAsync(request.UserId, client.AvatarId, cancellationToken);
        
        return MbResult<Unit>.Success(Unit.Value);
    }
}