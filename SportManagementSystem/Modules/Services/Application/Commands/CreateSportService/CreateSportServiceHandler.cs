using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Assets.Domain.Repositories;
using SportManagementSystem.Modules.Services.Domain.Entities;
using SportManagementSystem.Modules.Services.Domain.Repositories;

namespace SportManagementSystem.Modules.Services.Application.Commands.CreateSportService;

public class CreateSportServiceHandler(
    ISportServiceRepository sportServiceRepository,
    IBranchRepository branchRepository) : IMessageHandler<CreateSportServiceMessage, MbResult<long>>
{
    public async Task<MbResult<long>> Handle(CreateSportServiceMessage request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.GetByIdAsync(request.Request.BranchId, cancellationToken);
        if (branch is null)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Branch not found",
                status: StatusCodes.Status404NotFound,
                detail: "Филиал не найден"));
        }

        if (branch.AdminId != request.AdminId)
        {
            return MbResult<long>.Failure(new MbError(
                title: "Forbidden",
                status: StatusCodes.Status403Forbidden,
                detail: "Этот администратор не управляет филиалом"));
        }

        var sportService = new DbSportService
        {
            BranchId = request.Request.BranchId,
            Code = GenerateServiceCode(request.Request.Name),
            Name = request.Request.Name,
            Description = request.Request.Description,
            Category = request.Request.Category,
            IsActive = true,
            Created = DateTime.UtcNow
        };

        var result = await sportServiceRepository.CreateAsync(sportService, cancellationToken);
        return MbResult<long>.Success(result.Id);
    }

    private static string GenerateServiceCode(string? name)
    {
        var normalized = new string((name ?? string.Empty)
            .Trim()
            .ToUpperInvariant()
            .Where(char.IsLetterOrDigit)
            .Take(8)
            .ToArray());

        if (string.IsNullOrWhiteSpace(normalized))
        {
            normalized = "SERVICE";
        }

        var suffix = Guid.NewGuid().ToString("N")[..8];
        return $"{normalized}-{suffix}";
    }
}
