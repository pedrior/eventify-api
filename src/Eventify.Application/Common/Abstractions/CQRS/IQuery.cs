namespace Eventify.Application.Common.Abstractions.CQRS;

public interface IQuery<TResponse> : IRequest<ErrorOr<TResponse>>, IQueryRequest;