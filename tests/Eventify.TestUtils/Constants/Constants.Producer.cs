using Eventify.Domain.Producers.ValueObjects;

namespace Eventify.TestUtils.Constants;

public static partial class Constants
{
    public static class Producer
    {
        public static readonly ProducerId ProducerId = ProducerId.New();

        public static readonly ProducerDetails Details = new(
            name: "J.D Events",
            bio: """
                 We are a team of event producers that have been in the industry for over 20 years.
                 We have produced events for some of the biggest companies in the world and we are excited to
                 bring our expertise to your next event.
                 """);

        public static readonly ProducerContact Contact = new(
            email: "john@doe.com",
            phoneNumber: "+5581999999999");
    }
}