using Confluent.Kafka;

using Hypertheory.Documents;
using Hypertheory.KafkaUtils.Consumers;

using MongoDB.Driver;

using TrainingAcl.Adapters;
using TrainingAcl.Domain;

namespace TrainingAcl.Consumers;

public class UsersDocumentConsumer : ConsumeDocumentProtobufBackgroundService<UserKey, User>
{
    private readonly TrainingMongoDbAdapter _mongo;

    public UsersDocumentConsumer(ConsumerConfig consumerConfig, string topic, TrainingMongoDbAdapter mongo) : base(consumerConfig, topic)
    {
        _mongo = mongo;
    }

    protected override async void HandleConsumeLoop(ConsumeResult<UserKey, User> result)
    {
        var incomingUser = result.Value;
        var userToSave = new UserEntity
        {
            Id = incomingUser.UserId,
            FirstName = incomingUser.FirstName,
            LastName = incomingUser.LastName,
            Email = incomingUser.Email
        };

        var filter = Builders<UserEntity>.Filter.Where(u => u.Id == userToSave.Id);

        // This is how you do an Upsert in Mongo. QED. Word to your mother.
        await _mongo.Users.ReplaceOneAsync(filter, userToSave, new ReplaceOptions { IsUpsert = true });
        
      
    }
}
