using MongoDB.Driver;

using TrainingAcl.Domain;

namespace TrainingAcl.Adapters;

public class TrainingMongoDbAdapter
{
    private readonly IMongoCollection<UserEntity> _usersCollection;

    public TrainingMongoDbAdapter(string connectionString)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("training_db");
        _usersCollection = database.GetCollection<UserEntity>("users");
    }

    public IMongoCollection<UserEntity> Users { get { return _usersCollection; } }
}
