using MediatR;
using Microsoft.AspNetCore.Mvc;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Images.Application.Commands.DeleteImage;
using SportManagementSystem.Modules.Images.Application.Commands.UploadImage;
using SportManagementSystem.Modules.Images.Application.Queries.GetImage;
using SportManagementSystem.Modules.Images.Contracts.Requests;

namespace SportManagementSystem.Controllers;

public class ImagesController(IMediator mediator) : ApiControllerV1WithAuth
{
    /// <summary>
    /// Загрузить изображение.
    /// </summary>
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] UploadImageRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new UploadImageMessage(request), ct);

        return result.IsSuccess
            ? Created(nameof(GetById), new { id = result.Data })
            : ToActionResult(result);
    }

    /// <summary>
    /// Скачать изображение по идентификатору.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetImageMessage(id), ct);
        if (!result.IsSuccess)
        {
            return ToActionResult(result);
        }

        var file = result.Data!;
        return File(file.Content, file.ContentType, file.FileName);
    }

    /// <summary>
    /// Удалить изображение по идентификатору.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteImageMessage(id), ct);
        return ToActionResult(result);
    }
}
