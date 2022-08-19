using Microsoft.Extensions.Diagnostics.HealthChecks;

using MongoDB.Bson.Serialization.Attributes;

namespace TrainingAcl.Domain;

public class UserEntity
{
    [BsonElement("_id")]
    public string Id { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;    
    public string Email { get; set; } = string.Empty;
}
