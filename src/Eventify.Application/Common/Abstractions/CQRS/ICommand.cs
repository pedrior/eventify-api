namespace Eventify.Application.Common.Abstractions.CQRS;

public interface ICommand<TResponse> : IRequest<ErrorOr<TResponse>>;