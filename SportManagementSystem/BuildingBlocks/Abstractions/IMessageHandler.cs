using MediatR;

namespace SportManagementSystem.BuildingBlocks.Abstractions;

public interface IMessageHandler<in TMessage, TResponse> : IRequestHandler<TMessage, TResponse>
    where TMessage : IMessage<TResponse>;