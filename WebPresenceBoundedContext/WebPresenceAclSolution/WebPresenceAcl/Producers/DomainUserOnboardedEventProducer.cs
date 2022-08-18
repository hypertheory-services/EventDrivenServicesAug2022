using Hypertheory.KafkaUtils.Producers;
using Hypertheory.Events;
using Confluent.SchemaRegistry.Serdes;
using Hypertheory.KafkaUtils.Handlers;

namespace WebPresenceAcl.Producers;

public class DomainUserOnboardedEventProducer : EventProducer<UserOnboarded, ProtobufSerializer<UserOnboarded>>
{
    public DomainUserOnboardedEventProducer(ClientHandle handler, string registryUrl) : base(handler, registryUrl)
    {
    }
}
