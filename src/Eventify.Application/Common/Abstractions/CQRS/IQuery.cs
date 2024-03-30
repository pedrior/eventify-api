namespace Eventify.Application.Common.Abstractions.CQRS;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>, IQueryRequest;