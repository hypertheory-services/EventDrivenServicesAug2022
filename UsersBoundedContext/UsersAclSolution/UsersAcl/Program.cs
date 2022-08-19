using Confluent.Kafka;

using Hypertheory.KafkaUtils.Handlers;

using Microsoft.Extensions.Options;

using UsersAcl.Consumers;
using UsersAcl.Producers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOptions<ProducerConfig>().Bind(builder.Configuration.GetSection("Kafka:ProducerSettings"));

builder.Services.AddOptions<ConsumerConfig>().Bind(builder.Configuration.GetSection("Kafka:ConsumerSettings"));

var schemaUrl = builder.Configuration.GetConnectionString("schema-registry");
var userOnboardedTopicToConsume = "hypertheory-events-useronboarded";
var userDocumentToProduce = "hypertheory-documents-user";

builder.Services.AddSingleton<ClientHandle>(sp =>
{
    var config = sp.GetRequiredService<IOptions<ProducerConfig>>().Value;
    return new ClientHandle(config);
});

// Producers
builder.Services.AddSingleton<UserProducer>(sp =>
{
    var handle = sp.GetRequiredService<ClientHandle>();
    return new UserProducer(handle, schemaUrl);
});



// Consumer(s)
builder.Services.AddHostedService<UserOnboardedConsumer>(sp =>
{
    var config = sp.GetRequiredService<IOptions<ConsumerConfig>>().Value;
    
    var producer = sp.GetRequiredService<UserProducer>();
    var logger = sp.GetRequiredService<ILogger<UserOnboardedConsumer>>();
    logger.LogInformation("Blah To the Max", config);
    return new UserOnboardedConsumer(config, userOnboardedTopicToConsume, producer, logger, userDocumentToProduce);
});
var app = builder.Build();
app.MapGet("/", () => "This does no HTTP stuff!");

app.Run();
