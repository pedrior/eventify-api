using Eventify.Contracts.Producers.Responses;

namespace Eventify.Application.Producers.Queries.GetProfile;

public sealed record GetProducerProfileQuery : IQuery<ProducerProfileResponse>;