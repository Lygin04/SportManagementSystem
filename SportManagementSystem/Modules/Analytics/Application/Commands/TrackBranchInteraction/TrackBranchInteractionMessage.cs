using MediatR;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Abstractions;
using SportManagementSystem.Modules.Analytics.Contracts.Requests;

namespace SportManagementSystem.Modules.Analytics.Application.Commands.TrackBranchInteraction;

public record TrackBranchInteractionMessage(
    long? UserId,
    string? Role,
    TrackBranchInteractionRequest Request) : IMessage<MbResult<Unit>>;
