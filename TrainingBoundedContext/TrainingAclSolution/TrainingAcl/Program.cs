using Confluent.Kafka;

using Microsoft.Extensions.Options;

using MongoDB.Bson.Serialization.Conventions;

using TrainingAcl.Adapters;
using TrainingAcl.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("mongodb");

var conventionPack = new ConventionPack
{
    new CamelCaseElementNameConvention(),
    new IgnoreExtraElementsConvention(true),

};
ConventionRegistry.Register("default", conventionPack, t => true);



builder.Services.AddOptions<ConsumerConfig>().Bind(builder.Configuration.GetSection("Kafka:ConsumerSettings"));

builder.Services.AddSingleton<TrainingMongoDbAdapter>(sp =>
{
   
   
   return new TrainingMongoDbAdapter(connectionString);
});

builder.Services.AddHostedService<UsersDocumentConsumer>(sp =>
{
    var config = sp.GetRequiredService<IOptions<ConsumerConfig>>().Value;
    var mongo = sp.GetRequiredService<TrainingMongoDbAdapter>();
    return new UsersDocumentConsumer(config, "hypertheory-documents-user", mongo);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Run();

