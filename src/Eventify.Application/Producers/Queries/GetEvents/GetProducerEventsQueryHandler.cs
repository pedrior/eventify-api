using Eventify.Contracts.Producers.Responses;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.Producers.Queries.GetEvents;

internal sealed class GetProducerEventsQueryHandler(
    IUser user,
    IProducerRepository producerRepository
) : IQueryHandler<GetProducerEventsQuery, ProducerEventsResponse>
{
    public async Task<Result<ProducerEventsResponse>> Handle(GetProducerEventsQuery _,
        CancellationToken cancellationToken)
    {
        var producer = await producerRepository.GetByUserAsync(user.Id, cancellationToken);
        return producer is not null
            ? producer.Adapt<ProducerEventsResponse>()
            : throw new ApplicationException("Producer not found");
    }
}