using Confluent.SchemaRegistry.Serdes;
using Hypertheory.KafkaUtils;
using Hypertheory.KafkaUtils.Producers;
using Hypertheory.Events;
using Hypertheory.KafkaUtils.Handlers;

namespace WebPresenceAcl.Producers;

public class DomainEnrollmentRequestEventProducer : EventProducer
    <EnrollmentRequested, ProtobufSerializer<EnrollmentRequested>>
{
    public DomainEnrollmentRequestEventProducer(ClientHandle handler, string registryUrl) : base(handler, registryUrl)
    {
    }
}
