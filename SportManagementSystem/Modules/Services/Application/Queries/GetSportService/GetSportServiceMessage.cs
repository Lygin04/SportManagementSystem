using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Services.Domain.Entities;

namespace SportManagementSystem.Modules.Services.Application.Queries.GetSportService;

public record GetSportServiceMessage(long Id) : IMessage<MbResult<DbSportService>>;