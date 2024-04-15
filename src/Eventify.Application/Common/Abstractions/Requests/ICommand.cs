namespace Eventify.Application.Common.Abstractions.Requests;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;