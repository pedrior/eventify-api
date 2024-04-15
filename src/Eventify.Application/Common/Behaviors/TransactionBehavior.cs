using Eventify.Application.Common.Abstractions.Persistence;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Common.Behaviors;

internal sealed class TransactionBehavior<TRequest, TResponse>(
    IUnitOfWork unitOfWork,
    ILogger<TransactionBehavior<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse> where TRequest : ITransactional
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = request.GetType().Name;
        var transactionId = await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        logger.LogInformation("Transaction {TransactionId} has been started for request {RequestName}",
            transactionId, requestName);

        var response = await next();

        await unitOfWork.CommitTransactionAsync(cancellationToken);
        
        logger.LogInformation("Transaction {TransactionId} has been committed for request {RequestName}",
            transactionId, requestName);

        return response;
    }
}