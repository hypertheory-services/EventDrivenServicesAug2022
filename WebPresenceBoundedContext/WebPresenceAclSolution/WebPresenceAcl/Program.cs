using Confluent.Kafka;
using WebPresenceAcl.Producers;

using Hypertheory.KafkaUtils.Handlers;

using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddOptions<ProducerConfig>().Bind(builder.Configuration.GetSection("Kafka:ProducerSettings"));
var schemaUrl = builder.Configuration.GetConnectionString("schema-registry");

builder.Services.AddSingleton<ClientHandle>(sp =>
{
    var config = sp.GetRequiredService<IOptions<ProducerConfig>>().Value;
    return new ClientHandle(config);
});

builder.Services.AddSingleton<DomainUserOnboardedEventProducer>(sp =>
{
    var handle = sp.GetRequiredService<ClientHandle>();
    return new DomainUserOnboardedEventProducer(handle, schemaUrl);
});

builder.Services.AddSingleton<DomainEnrollmentRequestEventProducer>(sp =>
{
    var handle = sp.GetRequiredService<ClientHandle>();
    return new DomainEnrollmentRequestEventProducer(handle, schemaUrl);
});

builder.Services.AddControllers();

var app = builder.Build();
app.UseCloudEvents();
// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.MapSubscribeHandler();

app.Run();
