using Eventify.Domain.Producers.ValueObjects;
using Eventify.Domain.Users.ValueObjects;

namespace Eventify.TestUtils.Factories;

public static partial class Factories
{
    public static class Producer
    {
        public static Domain.Producers.Producer CreateProducer(
            ProducerId? producerId = null,
            UserId? userId = null,
            ProducerDetails? details = null,
            ProducerContact? contact = null,
            Uri? pictureUrl = null,
            Uri? websiteUrl = null)
        {
            return Domain.Producers.Producer.Create(
                producerId ?? Constants.Constants.Producer.ProducerId,
                userId ?? Constants.Constants.UserId,
                details ?? Constants.Constants.Producer.Details,
                contact ?? Constants.Constants.Producer.Contact,
                pictureUrl,
                websiteUrl);
        }
    }
}