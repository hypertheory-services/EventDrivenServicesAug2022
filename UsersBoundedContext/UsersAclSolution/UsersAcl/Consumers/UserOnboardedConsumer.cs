using Confluent.Kafka;

using Hypertheory.Documents;
using Hypertheory.Events;
using Hypertheory.KafkaUtils.Consumers;
using pb = Google.Protobuf.WellKnownTypes;
using UsersAcl.Producers;
using System.Text;

namespace UsersAcl.Consumers;

public class UserOnboardedConsumer : ConsumeEventProtobufBackgroundService<UserOnboarded>
{

    private readonly UserProducer _producer;
    private readonly ILogger<UserOnboardedConsumer> _logger;
    private readonly string _topicToProduce;
    public UserOnboardedConsumer(ConsumerConfig consumerConfig, string topic, UserProducer producer, ILogger<UserOnboardedConsumer> logger, string topicToProduce) : base(consumerConfig, topic)
    {
        _producer = producer;
        _logger = logger;
        _topicToProduce = topicToProduce;
    }

    protected override void HandleConsumeLoop(ConsumeResult<Null, UserOnboarded> result)
    {
        var incomingMessage = result.Message.Value;

        // Do all the real business work...

        var outgoingUser = new User
        {
            AccountRepId = "stacey@hypertheory.com",
            Email = incomingMessage.Email,
            FirstName = incomingMessage.FirstName,
            LastName = incomingMessage.LastName,
            UserId = incomingMessage.UserId,
            UserSince = pb.Timestamp.FromDateTime(DateTime.Now.ToUniversalTime())
        };

        var outgoingKey = new UserKey { Id = outgoingUser.UserId };

        var originHeaderValue = "users-acl-service";
        var message = new Message<UserKey, User>
        {
            Key = outgoingKey,
            Value = outgoingUser,
            Headers = new Headers { new Header("origin-service", Encoding.UTF8.GetBytes(originHeaderValue)) }
        };

        _producer.Produce(_topicToProduce, message);
    }
}
