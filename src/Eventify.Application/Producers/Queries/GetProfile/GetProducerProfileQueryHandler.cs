using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Contracts.Producers.Responses;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.Producers.Queries.GetProfile;

internal sealed class GetProducerProfileQueryHandler(
    IUser user,
    IProducerRepository producerRepository
) : IQueryHandler<GetProducerProfileQuery, ProducerProfileResponse>
{
    public async Task<Result<ProducerProfileResponse>> Handle(GetProducerProfileQuery query,
        CancellationToken cancellationToken)
    {
        var producer = await producerRepository.GetByUserAsync(user.Id, cancellationToken);
        return producer is not null
            ? producer.Adapt<ProducerProfileResponse>()
            : throw new ApplicationException("Producer not found");
    }
}