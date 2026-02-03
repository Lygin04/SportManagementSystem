using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Entities;
using SportManagementSystem.Modules.Assets.Domain.Repositories;

namespace SportManagementSystem.Modules.Assets.Application.Queries.GetEquipment;

public class GetEquipmentHandler(IEquipmentRepository equipmentRepository) : IMessageHandler<GetEquipmentMessage, MbResult<DbEquipment>>
{
    public async Task<MbResult<DbEquipment>> Handle(GetEquipmentMessage request, CancellationToken cancellationToken)
    {
        var result = await equipmentRepository.GetByIdAsync(request.Id, cancellationToken);

        if (result == null)
        {
            return MbResult<DbEquipment>.Failure(new MbError(
                title: "Equipment not found",
                status: StatusCodes.Status404NotFound,
                detail: "Оборудование не найдено"));
        }
        
        return MbResult<DbEquipment>.Success(result);
    }
}