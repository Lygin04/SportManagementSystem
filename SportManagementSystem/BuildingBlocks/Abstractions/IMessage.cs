using MediatR;

namespace SportManagementSystem.BuildingBlocks.Abstractions;

public interface IMessage<out TResponse> : IRequest<TResponse>;