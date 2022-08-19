using Confluent.SchemaRegistry.Serdes;

using Hypertheory.KafkaUtils.Producers;

namespace WebPresenceAcl.Producers;

public class DomainEnrollmentRequestEventProducer :EventProducer<Hypertheory.Events.EnrollmentRequested>, ProtobufDeserializer<Hypertheory.Events.EnrollmentRequested>
{
}
